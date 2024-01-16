open System


open System
open Akka.Actor
open Akka.FSharp
open WintailPart2.Actors


[<EntryPoint>]

    let main _ =
        let actorSystem = System.create "WinTailSystem" (Configuration.load())

        let consoleWriter = spawn actorSystem "ConsoleWriter"
                                     (actorOf consoleWriterActor)
        let validator = spawn actorSystem "Validator" (actorOf2 (validationActor consoleWriter))
                                     
        let consoleReader = spawn actorSystem "ConsoleReader"
                                     (actorOf2 (consoleReaderActor validator))

        

       
        consoleReader <! StartProcessing

        actorSystem.WhenTerminated.Wait()
        
        0
