module Exam.Part2.ChildActor
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages


//with mutable data
let MutableChildActor name (mailbox:Actor<ChildMessages>) =
 
    let mutable var = []// TODO: Make list or other mutable 0¨
    
    // TODO: Logic outside of loop
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
        
    let rec loop () =
        actor {
            let! msg = mailbox.Receive()   
             
            match msg with
            | ChildMessages.Start -> ()// TODO: Startprocess
            | Cancel ->
                        mailbox.Context.Stop(mailbox.Self)
            return! loop()
         }
    loop() 
    
    
// Without mutable

let ChildActor (mailbox:Actor<ChildMessages>) msg=
     
    // TODO: Logic outside of loop
    
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
                     
    match msg with
    | ChildMessages.Start -> ()// TODO: Startprocess
    | Cancel ->
                mailbox.Context.Stop(mailbox.Self)
   
    
                

    
        
    
     
    
    
    