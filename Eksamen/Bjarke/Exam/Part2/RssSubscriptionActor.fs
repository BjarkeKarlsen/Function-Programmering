module Exam.Part2.ParentActor
open System.Collections.Generic
open Exam.Part2.Messages
open Exam.Part2.ChildActor
open Akka.FSharp
open Akka.Actor
open System

    
let strategy () = Strategy.AllForOne(( fun error ->
    match error with
    | :? ArithmeticException -> Directive.Resume
    | :? NotSupportedException -> Directive.Stop
    | _ -> Directive.Restart), 10, TimeSpan.FromSeconds(30.))

let RssSubscriptionActor (mailbox: Actor<RssSubscriptionMessages>) =
    let mutable subscribers = []

    let output status message =
         select "/user/output" mailbox.Context.System <! status message   
         
    let rec loop() =
        actor {
          
            let! msg = mailbox.Receive()
            match msg with 
            | RssSubscriptionMessages.Subscribe url->
                          let childActor = mailbox.Context.Child("url")
                          match childActor.IsNobody() with
                          | false -> output ValidationError (sprintf $"Already exists subscriber to the url: %s{url}")
                          | true ->
                              spawnOpt mailbox.Context (url) (FeedActor url) [ SpawnOption.SupervisorStrategy(strategy()) ] |> ignore
                              subscribers <- url :: subscribers                    
            | RssSubscriptionMessages.UnSubscribe subscriber ->
                        match subscriber with
                        | name when List.contains name subscribers ->
                            output ValidationError "UnSubscribe"
                            subscribers <- subscribers |> List.filter (fun item -> item <> subscriber)
                        | _ -> output ValidationError (sprintf $"Url with name {subscriber} not found")
            | RssSubscriptionMessages.Refresh url ->
                        match url with
                        | name when List.contains name subscribers ->
                            mailbox.Context.Child name  <! RssFeedMessages.Refresh
                        | _ -> output ValidationError (sprintf $"Url with name {url} not found")
            | RssSubscriptionMessages.RefreshAll ->
                         for subscriber in subscribers do
                            mailbox.Context.Child subscriber  <! RssFeedMessages.Refresh 
            | RssSubscriptionMessages.GetAggregatedFeed  ->
                        match subscribers with
                        | subscribers when List.isEmpty subscribers ->
                            output ValidationError "Missing subscribes"
                        | subscribers ->
                            for subscriber in subscribers do
                                let childActor = mailbox.Context.Child(subscriber)
                                match childActor.IsNobody() with
                                | true -> printfn $"Child actor %s{subscriber} not found."
                                | false -> (childActor.Ask<string>(RssFeedMessages.GetData)).Result.ToString() |> output Success                           
            return! loop()
         }
    loop() 