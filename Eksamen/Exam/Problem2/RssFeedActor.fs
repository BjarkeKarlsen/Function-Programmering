module Problem2.RssFeedActor

open Akka.FSharp.Actors
open System
open Problem2.Messages
open Akka
open Akka.Actor
open Akka.FSharp
open Problem2.RssFetcherActor


let RssFeedActor (name: string) (mailbox:Actor<RssFeedMessages>) =
    let mutable data = ""
    let mutable state = Fresh ""
    
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
                output ConsoleLine $"Received data: {data}"                                  
                async {         
                    return data
                } |!> mailbox.Sender()       
            | RssFeedMessages.PoisonPill ->
                mailbox.Context.Stop(mailbox.Self)
                
            | _ -> output Error (sprintf "Unhandled message: %A" msg)

            return! loop()
         }
    loop() 
    
    
    // | Fresh _, RssFeedMessages.Refresh ->
    //             output ConsoleLine "Refreshing data..."
    //             state <- Fetching
    //             mailbox.Self <! GetData
    //             
    //         | Fresh data, RssFeedMessages.GetData ->
    //             output ConsoleLine $"Received data: {data}"                                  
    //             async {         
    //                 return data
    //             } |!> mailbox.Sender()
    //             state <- Fresh ""
    //             
    //             
    //         | Fetching, RssFeedMessages.GetData ->
    //             output ConsoleLine "Fetching data..."
    //
    //             data <- (mailbox.Context.Child(name).Ask<string>(FetcherMessages.Fetch)).Result.ToString()
    //             state <- Fresh data
    //             mailbox.Self <! GetData 
