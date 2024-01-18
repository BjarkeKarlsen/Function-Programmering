module Exam.Part2.ParentActor
open System.Collections.Generic
open Exam.Part2.Messages
open Exam.Part2.ChildActor
open Akka.FSharp


//
let RssSubscriptionActor (mailbox: Actor<RssSubscriptionMessages>) =
    let mutable subscribesUrls = []

    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    let rec loop() =
        actor {
          
            let! msg = mailbox.Receive()
            match msg with 
            | RssSubscriptionMessages.Subscribe url->
                          match url with
                            |name when List.contains name subscribesUrls ->
                                output ValidationError (sprintf $"Already exists url with the name %s{name}")
                            | _ -> 
                              spawn mailbox.Context (url) (MutableChildActor url)|> ignore
                              subscribesUrls <- url :: subscribesUrls                         
            | RssSubscriptionMessages.UnSubscribe url ->
                        match url with
                        | name when List.contains name subscribesUrls ->
                            mailbox.Context.Child name <! Start
                        | _ -> output ValidationError (sprintf $"Url with name {url} not found")
            | RssSubscriptionMessages.Refresh url ->
                        match url with
                        | name when List.contains name subscribesUrls ->
                            mailbox.Context.Child name <! Start
                        | _ -> output ValidationError (sprintf $"Url with name {url} not found")
            | RssSubscriptionMessages.RefreshAll ->
                         printfn "RefreshAll"
                         //mailbox.Context.Child  <! Start
            | RssSubscriptionMessages.GetAggregatedFeed  ->
                        printfn "getAggregatedFeed"      
            return! loop()
         }
    loop() 