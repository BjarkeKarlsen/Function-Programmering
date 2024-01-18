module Exam.Part2.ParentActor
open System.Collections.Generic
open Exam.Part2.Messages
open Exam.Part2.ChildActor
open Akka.FSharp


//
let ParentActor (mailbox: Actor<ParentMessages>) =
    let mutable exampleList = []

    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    let rec loop() =
        actor {
          
            let! msg = mailbox.Receive()
            match msg with 
            | CreateChild name->
                          match name with
                            |name when List.contains name exampleList ->
                                output ValidationError (sprintf $"Already exists ticket seller with the name %s{name}")
                            | _ -> 
                              spawn mailbox.Context (name) (MutableChildActor name)|> ignore
                              exampleList <- name :: exampleList                         
            | CallChild name ->
                        match name with
                        | name when List.contains name exampleList ->
                            mailbox.Context.Child name <! Start
                        | _ -> output ValidationError (sprintf $"TicketSeller with name {name} not found")
           
            return! loop()
         }
    loop() 