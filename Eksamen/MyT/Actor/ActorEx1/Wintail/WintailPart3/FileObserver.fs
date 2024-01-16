module WintailPart3.FileObserver
open Akka.Actor
open Akka.FSharp
open System.IO
open System


type TailActorMessages =
    | FileWrite of string
    | FileError of string*string
    | InitialRead of string*string
    
// FileObserver.fs
type FileObserver(tailActor: IActorRef, path: string) =
    let fileDir = Path.GetDirectoryName path
    let fileNameOnly = Path.GetFileName path
    let mutable watcher = null: FileSystemWatcher
    
    let onChanged (e: FileSystemEventArgs) =
        if (e.ChangeType = WatcherChangeTypes.Changed) then
            tailActor <! TailActorMessages.FileWrite e.Name
        else
            ()
    
    let onError (e: ErrorEventArgs) =
        tailActor <! TailActorMessages.FileError (fileNameOnly, e.GetException().Message)
    member this.Start() =
        watcher <- new FileSystemWatcher(fileDir, fileNameOnly)
        watcher.NotifyFilter <- NotifyFilters.FileName ||| NotifyFilters.LastWrite
        watcher.Changed.Add onChanged
        watcher.Error.Add onError
        watcher.EnableRaisingEvents <- true
        
    interface IDisposable with
        member this.Dispose () = watcher.Dispose()
        
        
// Actors.fs
