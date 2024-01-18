module Exam.Part2.FetcherActor

open System
open Akka.FSharp
open Exam.Part2.Messages

let random = Random()

let createRandomItem () =
    let randomString length =
        let chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        let stringBuilder = System.Text.StringBuilder()
        for _ in 1..length do
            stringBuilder.Append(chars.[random.Next(chars.Length)])
        stringBuilder.ToString()

    {
        Title = $"Example entry %s{randomString 5}"
        Description = $"Here is some text containing an interesting description. %s{randomString 10}"
        Link = $"http://www.example.com/blog/post/%d{random.Next(1, 100)}"
        Guid = System.Guid.NewGuid().ToString()
        PubDate = DateTime.UtcNow
    }
    
let createItemList count = [ for _ in 1..count -> createRandomItem() ]

let FetcherActor (url: string) (mailbox: Actor<FetcherMessages>) =
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            |   FetcherMessages.Fetch ->
                Threading.Thread.Sleep(random.Next(0, 5000))
                
                let itemList = createItemList 10

                if random.NextDouble() < 0.5 then
                    invalidOp "Could not fetch"

            return! loop()
         }
    loop() 