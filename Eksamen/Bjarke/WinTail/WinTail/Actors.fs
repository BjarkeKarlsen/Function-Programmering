module Wintail.Actors
open Akka.Actor
open Akka.FSharp
open System
open System.IO
open System.Text

type ConsoleReaderMessages =
    | ContinueProcessing
    | StartProcessing
    | ExitCommand
    
type ConsoleWriterMessages =
    | InputSuccess of string
    | InputError of string
    | ValidationError of string
    
type ValidationMessage =
    | FileUri of string

type CoordinateMessage =
    | StartTail of string*IActorRef
    
type TailMessage =
    | FileWrite of string
    | FileError of string*string
    | InitialRead of string*string
    
let consoleReaderActor validator (mailbox: Actor<ConsoleReaderMessages>) msg =

    match msg with
     | StartProcessing ->
         printfn "Please provide the URI of a log file on disk.\n"
         mailbox.Self <! ContinueProcessing
     | ContinueProcessing ->
         let line = Console.ReadLine()
         match line.ToLower() with
         | "exit" -> mailbox.Self <! ExitCommand
         | _ ->
             validator <! ValidationMessage.FileUri line   
     | ExitCommand ->
         mailbox.Context.System.Terminate () |> ignore
    

let consoleWriterActor msg =
      let printColored (text: string) color =
         Console.ForegroundColor <- color
         Console.WriteLine(text)
         Console.ResetColor()
      
      match box msg with
      | :? ConsoleWriterMessages as messages ->
            match messages with
            | InputSuccess text -> printColored text ConsoleColor.Green
            | InputError text -> printColored text ConsoleColor.Red
            | ValidationError text -> printColored text ConsoleColor.Red
      | _ -> printColored (msg.ToString()) ConsoleColor.Yellow
 
let fileValidationActor writer coordinator (mailbox: Actor<ValidationMessage>) msg =    
    match msg with
    | FileUri text ->
            match text with       
                |  text when String.IsNullOrEmpty text ->
                    writer <! ConsoleWriterMessages.InputError "Input was blank. Please try again\n"
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
                |  text when (File.Exists text) ->
                    writer <! ConsoleWriterMessages.InputSuccess  (sprintf $"Starting processing for %s{text}" )
                    coordinator <! CoordinateMessage.StartTail (text, writer)
                | text ->
                    writer <! ConsoleWriterMessages.ValidationError $"'%s{text}' is not an existing uri on disk"
                    mailbox.Sender() <! ConsoleReaderMessages.ContinueProcessing
            
    
type FileObserver(tailActor: IActorRef, path: string) =
    let fileDir = Path.GetDirectoryName path
    let fileNameOnly = Path.GetFileName path
    let mutable watcher = null: FileSystemWatcher
    
    let onChanged (e: FileSystemEventArgs) =
        if (e.ChangeType = WatcherChangeTypes.Changed) then
            tailActor <! TailMessage.FileWrite e.Name
        else
            ()
    
    let onError (e: ErrorEventArgs) =
        tailActor <! TailMessage.FileError (fileNameOnly, e.GetException().Message)
    member this.Start() =
        watcher <- new FileSystemWatcher(fileDir, fileNameOnly)
        watcher.NotifyFilter <- NotifyFilters.FileName ||| NotifyFilters.LastWrite
        watcher.Changed.Add onChanged
        watcher.Error.Add onError
        watcher.EnableRaisingEvents <- true
        
    interface IDisposable with
        member this.Dispose () = watcher.Dispose()
        
        
// Actors.fs
let tailActor (filePath: string) (reporter: IActorRef) (mailbox: Actor<_>) =
        let path = Path.GetFullPath(filePath)
        let observer = new FileObserver(mailbox.Self, path)
        do observer.Start ()
        let fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        let fileStreamReader = new StreamReader(fileStream, Encoding.UTF8)
        
        let text = fileStreamReader.ReadToEnd()
        
        mailbox.Defer <| fun () ->
            (observer :> IDisposable).Dispose()
            (fileStreamReader :> IDisposable).Dispose()
            (fileStream :> IDisposable).Dispose()
        
        mailbox.Self <! InitialRead (path, text)
        
        let rec loop () = actor {
            let! message = mailbox.Receive ()            
            match message with
            | FileWrite _ ->
                let text = fileStreamReader.ReadToEnd()
                reporter <! text
            | FileError (_, reason) ->
                reporter <! printf $"Tail error %s{reason}"
            | InitialRead (_, msg) ->
                printfn $"%s{msg}"
                reporter <! msg
            return! loop ()
        }
        loop ()
                
let fileCoordinatorActor (mailbox: Actor<CoordinateMessage>) msg =
    match msg with
    | StartTail (path, reporterActor) ->
        spawn mailbox.Context "tailActor" (tailActor path reporterActor) |> ignore
