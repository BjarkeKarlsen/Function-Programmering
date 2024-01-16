
open System


open System
open Akka.Actor
open Akka.FSharp
open WintailPart3.Actors
open WintailPart3.FileObserver


[<EntryPoint>]

    let main _ =
        let actorSystem = System.create "WinTailSystem" (Configuration.load())

        let consoleWriter = spawn actorSystem "ConsoleWriter"
                                     (actorOf consoleWriterActor)
        
                                     
        let coordinator = spawn actorSystem "Coordinater" (actorOf2 (fileCoordinationActor))                     

        let validator = spawn actorSystem "Validator" (actorOf2 (fileValidationActor coordinator consoleWriter ))

        let consoleReader = spawn actorSystem "ConsoleReader"
                                     (actorOf2 (consoleReaderActor validator))

        

    
        consoleReader <! StartProcessing

        actorSystem.WhenTerminated.Wait()
        
        0
