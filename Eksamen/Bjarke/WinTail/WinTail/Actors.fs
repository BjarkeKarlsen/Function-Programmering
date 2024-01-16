module Wintail.Actors
open Akka.FSharp
open System

type ConsoleReaderMessages =
    | ContinueProcessing
    | StartProcessing
    | ExitCommand
    
type ConsoleWriterMessages =
    | InputSuccess of string
    | InputError of string
    | Command of string
    
type ValidationMessage =
    | CommandMsg of string
    
let consoleReaderActor validator (mailbox: Actor<ConsoleReaderMessages>) msg =

    match msg with
     | StartProcessing ->
         validator <! ValidationMessage.CommandMsg "Starting to write to writer"
     | ContinueProcessing ->
         let line = Console.ReadLine()
         match line.ToLower() with
         | "exit" -> mailbox.Self <! ExitCommand
         | _ ->
             validator <! ValidationMessage.CommandMsg line   
     | ExitCommand ->
         printf ("Exiting")
         mailbox.Context.System.Terminate () |> ignore
    

let consoleWriterActor msg =
      let printColored (text: string) color =
         Console.ForegroundColor <- color
         Console.WriteLine(text)
         Console.ResetColor()
      
      match msg with  
        | InputSuccess text -> printColored text ConsoleColor.Green
        | InputError text -> printColored text ConsoleColor.Red
        | Command text -> printColored text ConsoleColor.DarkYellow
 
let validationActor writer (mailbox: Actor<ValidationMessage>) msg =
    match msg with
    | CommandMsg path ->
        if String.length path % 2 = 0 then
                writer <! InputSuccess path
                mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
            else   
                writer <! InputError path
                mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing