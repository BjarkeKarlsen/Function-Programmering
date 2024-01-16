module WintailPart2.Actors

open System.ComponentModel.Design
open Akka.Actor
open Akka.FSharp
open System

type ConsoleReaderMessages =
    | StartProcessing
    | ContinueProcessing 
    | ExitCommand
    
type ConsoleWriterMessages =
    | InputError of string
    | InputSuccess of string

type ValidationMessages =
    | Start 
    | Command of string
    
let consoleReaderActor  validator (mailbox: Actor<ConsoleReaderMessages>) msg =
    match msg with
     | ExitCommand -> mailbox.Self <! mailbox.Context.System.Terminate () |> ignore  
     | StartProcessing ->
         validator <! ValidationMessages.Start
     | ContinueProcessing ->
        let line = Console.ReadLine()
        match line.ToLower() with
         | "exit" -> mailbox.Self <! ExitCommand
         | _ -> validator <! ValidationMessages.Command line

  

let validationActor writer (mailbox: Actor<ValidationMessages>) msg =
        match msg with
        | Start ->
            mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
        | Command text ->
            if String.length text % 2 = 0 then
                    writer <! ConsoleWriterMessages.InputSuccess text
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
                else
                    writer <! ConsoleWriterMessages.InputError text
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing

        

     // | ContinueProcessing ->
     //     let line = Console.ReadLine()
     //     match line.ToLower() with
     //     | "exit" -> mailbox.Self <! ExitCommand
     //     | _ ->
     //         validator <! ValidationMessages.Command line
     //         mailbox.Self <! ContinueProcessing
     //
 
let consoleWriterActor msg =
      let printColored (text: string) color =
         Console.ForegroundColor <- color
         Console.WriteLine(text)
         Console.ResetColor()
      
      match msg with
        | ConsoleWriterMessages.InputError text -> printColored text ConsoleColor.Red
        | ConsoleWriterMessages.InputSuccess text -> printColored text ConsoleColor.Green
        
     
