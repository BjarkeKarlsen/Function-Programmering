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
 
    let mutable data = []// TODO: Make list or other mutable 0¨
    
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
    
    let generateRandomItem () =
        // Generate a unique identifier for the GUID
        let guid = Guid.NewGuid().ToString()
        // Generate a random date within a specific range
        let random = Random()
        let pubDate = DateTime.UtcNow.AddDays(-random.Next(1, 365))
        // Generate random text for title and description
        let title = $"Example entry {random.Next(1, 100)}"
        let description = $"Here is some text containing an interesting description for {title}"

        // Construct the XML string for the item
        let xml =
            $"<item>\n <title>{title}</title>\n <description>{description}</description>\n" +
            $"<link>http://www.example.com/blog/post/{guid}</link>\n" +
            $"<guid isPermaLink=\"false\">{guid}</guid>\n" +
            $"<pubDate>{pubDate:R}</pubDate>\n</item>"

        xml
        
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | FetcherMessages.Fetch ->
                async {
                let randomItem = generateRandomItem()
                output ConsoleLine $"Subscription: {name}\n"
                output Success ($"Generated random item:\n{randomItem}" )
                return randomItem
                } |!> mailbox.Sender()
                
                // let data = printfn "Create random item"
                // mailbox.Context.Sender <! RssFeedMessages.GetData data
                //let data = "hi"
                //mailbox.Context.Parent <! GetData_ data
                //send data to parent
                // fetch
                // mailbox.Context.Sender <! RssFeedMessages.GetData 
            return! loop()
         }
    loop() 


let RssFeedActor (name: string) (mailbox:Actor<RssFeedMessages>) =
    let mutable data = ""
    
    spawn mailbox.Context (name) (RssFetchActor name)|> ignore
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
        
    let rec loop () =
        actor {
           
            let! msg = mailbox.Receive()   
             
            match msg with
            | RssFeedMessages.Refresh ->
                output ConsoleLine "Refreshing data..."
                data <- (mailbox.Context.Child(name).Ask<string>(FetcherMessages.Fetch)).Result.ToString()
                
            | RssFeedMessages.GetData ->
                async {         
                    return data
                } |!> mailbox.Sender()
            | RssFeedMessages.PoisonPill ->
                mailbox.Context.Stop(mailbox.Self)
                
            | _ -> output Error (sprintf "Unhandled message: %A" msg)

            return! loop()
         }
    loop() 
    


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
                    |
                
                //Output an aggregated RSS feed containing merged data from all rssFeedActors 
            | _ -> output Error (sprintf "Unhandled message: %A" msg)
                
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
            output ConsoleLine "Write an URL you want to unsubscribe to"
            let name = Console.ReadLine()

            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Unsubscribe name

            mailbox.Self <! StartProcessing
        | "3" ->
            output ConsoleLine "Write an URL you want to refresh"
            let name = Console.ReadLine()

            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Refresh name
            mailbox.Self <! StartProcessing

        |"4" ->
            output ConsoleLine "Refreshing all"
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.RefreshAll
            mailbox.Self <! StartProcessing
        | "5" ->
            output ConsoleLine "Get all data"
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.GetAggregatedFeed
            mailbox.Self <! StartProcessing
        | "6" ->
            output ConsoleLine "Exiting..."
            mailbox.Context.Stop(mailbox.Self) |> ignore
        | _ ->
            output Error "Please enter something valid"
            mailbox.Self <! StartProcessing
    | _  -> output Error "Not started"
       