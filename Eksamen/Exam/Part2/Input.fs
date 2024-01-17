module Exam.Part2.Input

open System
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages

let tryToInt (input: string) =
    match Int32.TryParse(input) with
    | (true, value) -> Some value
    | _ -> None



let inputActor (mailbox: Actor<InputMessages>) msg =
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    match msg with
    | StartProcessing ->
        output Menu "Create process"
        output Menu "Exit process "
        
        let line = Console.ReadLine()
        
        match line with
        | "1" -> 
            mailbox.Self <! CreateProcessing
        | "2" -> 
            mailbox.Self <! ExitProcessing
        | _ ->
            output ValidationError "Please enter something valid"
            mailbox.Self <! StartProcessing
    | CreateProcessing ->
        output ConsoleLine "Enter a ticket seller you want to create"
        let line = Console.ReadLine()
        match line with
        | (line) when String.IsNullOrEmpty line ->
            output ValidationError "Not valid"
            mailbox.Self <! StartProcessing
        | (line)  ->
            select "/user/parentActor" mailbox.Context.System <! CreateChild line
            mailbox.Self <! StartProcessing
       
    | ExitProcessing -> failwith "todo"
    | _ -> output NullInputError "oopsies :)"