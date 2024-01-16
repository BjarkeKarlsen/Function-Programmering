// Actors.fs

module WinTail.Actors

open System
open Akka.Actor
open Akka.FSharp

type SenderMessages =
    | Start
    | Continue
    | Write

type WriterMessages =
    | InputSuccess of string
    | InputError of string
    | Command of string


let consoleReaderActor writer (mailbox: Actor<SenderMessages>) msg =
           
    match msg with
    | Start  -> 
        writer <! WriterMessages.Command "Starting to write to writer: "
        mailbox.Self <! Write 
    | Write ->
        writer <! WriterMessages.Command (sprintf "Start writing text to " )
        writer <! WriterMessages.InputError (sprintf $"%s{Console.ReadLine()} " )
        mailbox.Self <! Write
        


let consoleWriter msg =
    let print color (msg: string) =
        Console.ForegroundColor <- color
        Console.WriteLine(msg)
        Console.ResetColor()
    
    match msg with
        | InputSuccess a -> print ConsoleColor.Red a
        | InputError a -> print ConsoleColor.Green a
        | Command a -> print ConsoleColor.Red a
       
    // Types security using box    
    // match box msg with
    // | :? WriterMessages as messages ->
    //     match messages with
    //     | InputSuccess a -> print ConsoleColor.Red a
    //     | InputError a -> print ConsoleColor.Green a
    //     | Command a -> print ConsoleColor.Red a
    // | _ -> print ConsoleColor.Yellow (msg.ToString())
