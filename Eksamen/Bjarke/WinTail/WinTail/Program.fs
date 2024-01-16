
open Akka.FSharp
open Akka.Actor
open WinTail.Actors
open System

[<EntryPoint>]
let main argv =
    //use system = System.create "MyActorSystem" (Configuration.load())
    
    let strategy () = Strategy.OneForOne(( fun error ->
    match error with
    | :? ArithmeticException -> Directive.Resume
    | :? NotSupportedException -> Directive.Stop
    | _ -> Directive.Restart), 10,
            TimeSpan.FromSeconds(30.))
    
    let actorSystem = create "MyActorSystem"
                                     (Configuration.load())
    
    let writer = spawn actorSystem "WriterActor" (actorOf consoleWriter)
    
    let reader = spawn actorSystem "ReaderActor" (actorOf2 (consoleReaderActor writer))
    
    writer <! Start
    reader <! Start
    
    actorSystem.WhenTerminated.Wait ()
    
    0