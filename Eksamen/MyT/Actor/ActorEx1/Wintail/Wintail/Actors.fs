module Wintail.Actors
open System.ComponentModel.Design
open Akka.Actor
open Akka.FSharp
open System

type ConsoleReaderMessages =
    | ContinueProcessing
    | StartProcessing
    | ExitCommand
    
type ConsoleWriterMessages =
    | Command of string
let consoleReaderActor writer (mailbox: Actor<ConsoleReaderMessages>) msg =
    
    match msg with
     | StartProcessing ->
         writer <! ConsoleWriterMessages.Command "Starting to write to writer"
         mailbox.Self <! ContinueProcessing
     | ContinueProcessing ->
         let line = Console.ReadLine()
         match line.ToLower() with
         | "exit" -> mailbox.Self <! ExitCommand
         | _ ->
             writer <! ConsoleWriterMessages.Command line
             mailbox.Self <! ContinueProcessing        
     | ExitCommand ->
         printf ("Exiting")
         mailbox.Context.System.Terminate () |> ignore
    

let consoleWriterActor msg =
      let printColored (text: string) color =
         Console.ForegroundColor <- color
         Console.WriteLine(text)
         Console.ResetColor()
      
      match msg with
        | ConsoleWriterMessages.Command text ->
            if String.length text % 2 = 0 then
                    printColored text ConsoleColor.Green
                else
                    printColored text ConsoleColor.Red
            
     
