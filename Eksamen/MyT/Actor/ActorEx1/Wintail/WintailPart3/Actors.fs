module WintailPart3.Actors
open System.IO
open System.ComponentModel.Design
open Akka.Actor
open Akka.FSharp
open System
open FileObserver
open System.Text

type ConsoleReaderMessages =
    | StartProcessing
    | ContinueProcessing 
    | ExitCommand
    
type ConsoleWriterMessages =
    | InputError of string
    | InputSuccess of string

type FileValidationMessages =
    | Empty 
    | FileUri of string
    | Continue
    
type CoordinateMessages =
    | StartTail of string * IActorRef
    
let consoleReaderActor validator (mailbox: Actor<ConsoleReaderMessages>) msg =
    match msg with
     | ExitCommand -> mailbox.Self <! mailbox.Context.System.Terminate () |> ignore  
     | StartProcessing ->
         printfn "Please provide the URI of a log file"
         mailbox.Self <! ContinueProcessing
         //validator <! ValidationMessages.Start
     | ContinueProcessing ->
        let line = Console.ReadLine()
        match line.ToLower() with
         | "exit" -> mailbox.Self <! ExitCommand
         | _ -> validator <! FileValidationMessages.FileUri line
        

  
 
      
let fileValidationActor coordinator writer (mailbox: Actor<FileValidationMessages>) msg =
        match msg with
        | FileUri text ->
            match text with 
                | text when String.IsNullOrEmpty text ->
                    writer <! ConsoleWriterMessages.InputError "String is empty"
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
                | text when File.Exists text ->
                    writer <! ConsoleWriterMessages.InputSuccess (sprintf"Path is found")
                    coordinator <! CoordinateMessages.StartTail (text, writer)
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
                | _ ->
                    writer <! ConsoleWriterMessages.InputError "Path doesn't exist"
                    let fullpath = Path.GetFullPath text
                    writer <! ConsoleWriterMessages.InputSuccess fullpath
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
         
        // | FileUri text ->
        //     if String.length text % 2 = 0 then
        //             writer <! ConsoleWriterMessages.InputSuccess text
        //             mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
        //         else
        //             writer <! ConsoleWriterMessages.InputError text
        //             mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing


     
let tailActor (filePath: string) (reporter: IActorRef) (mailbox: Actor<_>) =
        let fullpath = Path.GetFullPath filePath
        let observer = new FileObserver(mailbox.Self, fullpath)
        
        do observer.Start ()
        let fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        let fileStreamReader = new StreamReader(fileStream, Encoding.UTF8)
        
        let text = fileStreamReader.ReadToEnd()
        
        mailbox.Defer <| fun () ->
            (observer :> IDisposable).Dispose()
            (fileStreamReader :> IDisposable).Dispose()
            (fileStream :> IDisposable).Dispose()
        
        mailbox.Self <! InitialRead (filePath, text)
           

        // TODO open file and read initial data
        
        let rec loop () = actor {
            let! msg = mailbox.Receive()
            match msg with
            | FileWrite _ ->
                let text = fileStreamReader.ReadToEnd()
                reporter <! text
            | FileError (_, reason) ->
                reporter <! printf "Tail error %s" reason
            | InitialRead (_, msg) ->
                reporter <! msg
            
            // TODO match message and handle
            
            return! loop ()
        }
        loop ()    
     // | ContinueProcessing ->
     //     let line = Console.ReadLine()
     //     match line.ToLower() with
     //     | "exit" -> mailbox.Self <! ExitCommand
     //     | _ ->
     //         validator <! ValidationMessages.Command line
     //         mailbox.Self <! ContinueProcessing
     //
 
let fileCoordinationActor (mailbox: Actor<CoordinateMessages>) msg =
 match msg with
  | StartTail (path,fileReaderActor) ->
      spawn mailbox.Context "tailActor" (tailActor path fileReaderActor)|> ignore
 
let consoleWriterActor msg =
      let printColored (text: string) color =
         Console.ForegroundColor <- color
         Console.WriteLine(text)
         Console.ResetColor()
      
      match box msg with
      | :? ConsoleWriterMessages as messages ->
        match messages with 
        | ConsoleWriterMessages.InputError text -> printColored text ConsoleColor.Red
        | ConsoleWriterMessages.InputSuccess text -> printColored text ConsoleColor.Green
      | _  ->  printColored (msg.ToString()) ConsoleColor.Yellow



