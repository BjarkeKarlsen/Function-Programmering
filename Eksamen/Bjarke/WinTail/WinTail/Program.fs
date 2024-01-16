
open Akka.FSharp
open Akka.Actor
open Wintail.Actors
open System

[<EntryPoint>]
let main argv =
    let strategy () = Strategy.OneForOne(( fun error ->
    match error with
    | :? ArithmeticException -> Directive.Resume
    | :? NotSupportedException -> Directive.Stop
    | _ -> Directive.Restart), 10,
            TimeSpan.FromSeconds(30.))
    
    let actorSystem = create "MyActorSystem"
                                     (Configuration.load())
    
    let writer = spawn actorSystem "WriterActor" (actorOf consoleWriterActor)
    
    let coordinator = spawn actorSystem "coordinatorActor" (actorOf2 fileCoordinatorActor)
      //                             [ SpawnOption.SupervisorStrategy(strategy()) ]
    
    let validator = spawn actorSystem "validatorActor" (actorOf2 (fileValidationActor writer coordinator))
    
    let reader = spawn actorSystem "ReaderActor" (actorOf2 (consoleReaderActor validator))
    
    reader <! StartProcessing
    
    actorSystem.WhenTerminated.Wait ()
    
    0