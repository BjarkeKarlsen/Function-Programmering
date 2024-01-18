module Exam.Part2.ParentActor
open System.Collections.Generic
open Exam.Part2.Messages
open Exam.Part2.ChildActor
open Akka.FSharp
open Akka.Actor

let RssSubscriptionActor (mailbox: Actor<RssSubscriptionMessages>) =
    let mutable subscribers = []

    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    let rec loop() =
        actor {
          
            let! msg = mailbox.Receive()
            match msg with 
            | RssSubscriptionMessages.Subscribe url->
                          match url with
                            |name when List.contains name subscribers ->
                                output ValidationError (sprintf $"Already exists url with the name %s{name}")
                            | _ -> 
                              spawn mailbox.Context (url) (FeedActor url)|> ignore
                              subscribers <- url :: subscribers                         
            | RssSubscriptionMessages.UnSubscribe url ->
                        match url with
                        | name when List.contains name subscribers ->
                            output ValidationError "UnSubscribe"
                        | _ -> output ValidationError (sprintf $"Url with name {url} not found")
            | RssSubscriptionMessages.Refresh url ->
                        match url with
                        | name when List.contains name subscribers ->
                            mailbox.Context.Child name  <! RssFeedMessages.Refresh
                        | _ -> output ValidationError (sprintf $"Url with name {url} not found")
            | RssSubscriptionMessages.RefreshAll ->
                         for subscriber in subscribers do
                            mailbox.Context.Child subscriber  <! RssFeedMessages.Refresh 
            | RssSubscriptionMessages.GetAggregatedFeed  ->
                        output Success "Aggregate"
                        for subscriber in subscribers do
                            let childActor = mailbox.Context.Child(subscriber)
                            match childActor.IsNobody() with
                            | true -> printfn "Child actor 'url' not found."
                            | false -> (childActor.Ask<string>(RssFeedMessages.GetData)).Result.ToString() |> output Success                           
            return! loop()
         }
    loop() 