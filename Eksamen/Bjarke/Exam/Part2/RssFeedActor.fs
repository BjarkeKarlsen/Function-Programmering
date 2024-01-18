module Exam.Part2.ChildActor
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages
open Exam.Part2.FetcherActor


//with mutable data
let FeedActor name (mailbox:Actor<RssFeedMessages>) =
    0
    //let mutable var = []// TODO: Make list or other mutable 0¨
    
    // TODO: Logic outside of loop
    //let observer = new FetcherActor(mailbox.Self, fullpath)
        
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
    // let rec loop () =
    //     actor {
    //         let! msg = mailbox.Receive()   
    //          
    //         match msg with
    //         | Refresh -> 0
    //         | GetData -> 0
    //                     
    //         return! loop()
    //      }
    // loop() 
    //
    //
    
        
    
     
    
    
    