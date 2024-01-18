module Exam.Part2.ChildActor
open System.Runtime.CompilerServices
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages
open Exam.Part2.FetcherActor


//with mutable data
let FeedActor (url: string) (mailbox:Actor<RssFeedMessages>) =
    let mutable data = ""
    spawn mailbox.Context "url" (FetcherActor url) |> ignore
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
        
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | RssFeedMessages.Refresh ->
                let childActor = mailbox.Context.Child("url")
                match childActor.IsNobody() with
                | true -> printfn "Child actor 'url' not found."
                | false -> data <- (childActor.Ask<string>(FetcherMessages.Fetch)).Result.ToString()
                output Success data
            | RssFeedMessages.GetData  ->
                async {         
                    return data
                } |!> mailbox.Sender()
            return! loop()
         }
    loop() 

    
        

    
    