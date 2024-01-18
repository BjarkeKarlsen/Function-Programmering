module Problem2.RssSubscriptionActor
open System.Collections.Generic
open Problem2.Messages
open Problem2.CmdActorIdle
open Akka.FSharp
open System
open Akka.Actor
open Akka.FSharp
open Problem2.Messages

let RssFetchActor name (mailbox:Actor<FetcherMessages>) =
 
    let mutable var = []// TODO: Make list or other mutable 0¨
    
    // TODO: Logic outside of loop
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
        
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | FetcherMessages.Fetch ->
                printfn "Create random item"
                //let data = "hi"
                //mailbox.Context.Parent <! RssFeedMessages.GetData data
                // fetch
                // mailbox.Context.Sender <! RssFeedMessages.GetData 
            return! loop()
         }
    loop() 


let RssFeedActor name (mailbox:Actor<RssFeedMessages>) =
        
    // TODO: Logic outside of loop
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
        
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | CreateFetcher name ->
                spawn mailbox.Context ("RssFetchActor" + name) (RssFetchActor name)|> ignore
            | RssFeedMessages.Refresh name ->
                mailbox.Context.Child name <! FetcherMessages.Fetch
            | RssFeedMessages.GetData data ->
                printfn "GetData from feed"
                //mailbox.Context.Parent <! RssSubscriptionMessages.GetAggregatedFeed data                 
            | Cancel ->
                        mailbox.Context.Stop(mailbox.Self)
            return! loop()
         }
    loop() 
    

//
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
                        // | match name with feedactors if does spawn and sub
                        |name when List.contains name subscribedList ->
                            output Error (sprintf $"Already subscribed rss feed with the url %s{name}")
                        | _ -> 
                          spawn mailbox.Context ("RssFeedActor" + name) (RssFeedActor name)|> ignore
                          subscribedList <- name :: subscribedList
                          printfn $"{subscribedList}"
            | RssSubscriptionMessages.Unsubscribe name ->
                match name with
                        // | match name with feedactors if does spawn and sub
                        |name when List.contains name subscribedList ->
                            output ConsoleLine $"Take list of subscriptions and unsubscribe from url {name}"
                            let poppedSubscription = popSubscription name
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
                 printfn "Refresh all"
                 for name in subscribedList do
                            mailbox.Context.Child name <! RssFeedMessages.Refresh
                 //output Cons
                //Refresh all feeds
            | RssSubscriptionMessages.GetAggregatedFeed->
                for name in subscribedList do
                            mailbox.Context.Child name <! RssFeedMessages.GetData
                printfn "an aggregated RSS feed containing merged data from all rssFeedActors"
                
                //Output an aggregated RSS feed containing merged data from all rssFeedActors 
              
                
            return! loop()
         }
    loop()




let tryToInt (input: string) =
    match Int32.TryParse(input) with
    | (true, value) -> Some value
    | _ -> None



let CmdActor (mailbox: Actor<CmdMessages>) msg =
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    match msg with
    | StartProcessing ->
        output Menu "Press 1 to subscribe to url"
        output Menu "Press 2 to unsubscribe to url "
        output Menu "Press 3 to refresh url"
        output Menu "Press 4 to refresh all"
        output Menu "Press 5 Get aggregated RSS Feed with data from all RSS Feeds"
        output Menu "Press 6 to Exit"
        
        let line = Console.ReadLine().ToLower()
        
        match line with 
        | "1" ->
            output ConsoleLine  "Write an URL you want to subscribe to"
            let name = Console.ReadLine()
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Subscribe name 
            mailbox.Self <! StartProcessing

        | "2" ->
            output ConsoleLine "write an URL you want to unsubscribe to"
            let name = Console.ReadLine()

            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Unsubscribe name

            mailbox.Self <! StartProcessing
        | "3" ->
            output ConsoleLine "write an URL you want to refresh"
            let name = Console.ReadLine()

            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Refresh name
            mailbox.Self <! StartProcessing

        |"4" ->
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.RefreshAll
            mailbox.Self <! StartProcessing
        | "5" -> 
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.GetAggregatedFeed
            mailbox.Self <! StartProcessing
        | "6" ->
            mailbox.Context.Stop(mailbox.Self) |> ignore
        | _ ->
            output Error "Please enter something valid"
            mailbox.Self <! StartProcessing
    | _  -> output Error "Not started"
       