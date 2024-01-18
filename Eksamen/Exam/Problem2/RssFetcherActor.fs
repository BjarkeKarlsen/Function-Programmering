module Problem2.RssFetcherActor
open System.Collections.Generic
open Problem2.Messages
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



