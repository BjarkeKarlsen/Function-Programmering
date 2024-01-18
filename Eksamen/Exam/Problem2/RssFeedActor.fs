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
    