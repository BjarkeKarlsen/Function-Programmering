module Problem2.Output


open System
open Akka.Actor
open Akka.FSharp
open Problem2.Messages


let OutputActor msg =
    let print color (msg: string) =
        Console.ForegroundColor <- color
        Console.WriteLine(msg)
        Console.ResetColor()
        
    match box msg with
    | :? ConsoleWriterMessages as messages ->
        match messages with
        | Success a -> print ConsoleColor.Green a
        | ConsoleLine a -> print ConsoleColor.White a
        | Menu a -> print ConsoleColor.Yellow a
        | Error a -> print ConsoleColor.Red a
    | _ -> print ConsoleColor.White (msg.ToString())