open System
open Akka.Actor
open Akka.FSharp
open Wintail.Actors


[<EntryPoint>]

    let main _ =
        let actorSystem = System.create "WinTailSystem" (Configuration.load())

        let consoleWriter = spawn actorSystem "ConsoleWriter"
                                     (actorOf consoleWriterActor)
                                     
        let consoleReader = spawn actorSystem "ConsoleReader"
                                     (actorOf2 (consoleReaderActor consoleWriter))

        

       
        consoleReader <! StartProcessing

        actorSystem.WhenTerminated.Wait()
        
        0