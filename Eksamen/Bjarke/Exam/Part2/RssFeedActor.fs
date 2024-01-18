module Exam.Part2.ChildActor
open System.Runtime.CompilerServices
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages
open Exam.Part2.FetcherActor


//with mutable data
let FeedActor (url: string) (mailbox:Actor<RssFeedMessages>) =
    let mutable data = ""
    let name = "FeedActor" + url
    spawn mailbox.Context name (FetcherActor url) |> ignore
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | RssFeedMessages.Refresh ->
                let childActor = mailbox.Context.Child(name)
                match childActor.IsNobody() with
                | true -> output ValidationError  $"Child actor %s{name} not found."
                | false ->
                    output Success $"Refreshing: %s{url}"
                    data <- (childActor.Ask<string>(FetcherMessages.Fetch)).Result.ToString()
            | RssFeedMessages.GetData  ->
                async {         
                    return data
                } |!> mailbox.Sender()
            return! loop()
         }
    loop() 

    
        

    
    