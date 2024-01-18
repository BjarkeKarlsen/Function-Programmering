module Problem2.RssSubscriptionActor
open Akka
open Akka.Actor
open Akka.FSharp.Actors
open Akka.FSharp
open System
open Problem2.Messages
open Problem2.RssFeedActor

let RssSubscriptionActor (mailbox: Actor<RssSubscriptionMessages>) =
    let mutable subscribedList = []
    // let mutable feedActors = []

    let output status message =
         select "/user/output" mailbox.Context.System <! status message
    
    let popSubscription subscription =
        let poppedSubscription, remainingSubscription =
            subscribedList
            |> List.partition (fun item -> item = subscription)

        subscribedList <- remainingSubscription 
        poppedSubscription
    

    let rec loop() =
        actor {
            let! msg = mailbox.Receive()
            match msg with
            | RssSubscriptionMessages.Subscribe name ->
                match name with
                        |name when List.contains name subscribedList ->
                            output Error (sprintf $"Already subscribed rss feed with the url %s{name}")
                        | _ ->
                          output ConsoleLine $"subscribing to {name}"
                          spawn mailbox.Context (name) (RssFeedActor name)|> ignore
                          subscribedList <- name :: subscribedList
                          output Success $"Subscriptions: {subscribedList}"
                          
            | RssSubscriptionMessages.Unsubscribe name ->
                match name with
                        // | match name with feedactors if does spawn and sub
                        |name when List.contains name subscribedList ->
                            output ConsoleLine $"Take list of subscriptions and unsubscribe from url {name}"
                            let poppedSubscription = popSubscription name
                         
                            mailbox.Context.Child name <! RssFeedMessages.PoisonPill
                            output Success $"Unsubscribed from: %A{poppedSubscription}"
                            output Success $"Remaining subscriptions: %A{subscribedList}"
                        | _ -> 
                          output Error (sprintf $"RSS Feed with URL {name} not found")
                                                  
            | RssSubscriptionMessages.Refresh name ->   
                match name with
                        | name when List.contains name subscribedList ->
                             mailbox.Context.Child name <! RssFeedMessages.Refresh
                        | _ -> output Error (sprintf $"RSS Feed with URL {name} not found")
            |RssSubscriptionMessages.RefreshAll ->
                 output ConsoleLine "Refresh all"
                 for name in subscribedList do
                        mailbox.Self <! RssSubscriptionMessages.Refresh name
                        
                 // for name in subscribedList do
                 //            mailbox.Context.Child name <! Refresh_
                 //output Cons
                //Refresh all feeds
            | RssSubscriptionMessages.GetAggregatedFeed ->
                output ConsoleLine "Merges data from all RSS Feeds"
                for name in subscribedList do
                    let data = mailbox.Context.Child(name).Ask<string>(RssFeedMessages.GetData).Result.ToString()
                    output Success data
                
                //Output an aggregated RSS feed containing merged data from all rssFeedActors 
            | _ -> output Error $"Unhandled message: %A{msg}"
                
            return! loop()
         }
    loop()


