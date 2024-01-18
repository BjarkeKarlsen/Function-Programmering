module Exam.Part2.ChildActor
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages
open Exam.Part2.FetcherActor


//with mutable data
let FeedActor (url: string) (mailbox:Actor<RssFeedMessages>) =
    let mutable data = []// TODO: Make list or other mutable 0¨
    spawn mailbox.Context "url" (FetcherActor url) |> ignore 
        
  
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | RssFeedMessages.Refresh ->
                mailbox.Context.Child "url" <! FetcherMessages.Fetch 
            | RssFeedMessages.GetData replyChannel  ->
                replyChannel.Reply data
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
    
    
    