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
                // Use an AsyncReplyChannel to collect data from FetcherActor
                mailbox.Context.Child "url" <! FetcherMessages.Fetch
                
            | RssFeedMessages.GetData replyChannel  ->
                printfn "Martin what youy duing"
                output Success (sprintf $"%A{data}")
                //replyChannel.Reply data
            | Reply data_ ->
                data <- data_
            return! loop()
         }
    loop() 

    
        
    
    // do observer.Start ()
    // let output status message =
    //      select "/user/output" mailbox.Context.System <! status message
    //      
    // mailbox.Defer <| fun () ->
    //     (observer :> IDisposable).Dispose()
    //     (fileStreamReader :> IDisposable).Dispose()
    //     (fileStream :> IDisposable).Dispose()
    //
    // mailbox.Self <! InitialRead (filePath, text)
    //        
    //   
    
    
    