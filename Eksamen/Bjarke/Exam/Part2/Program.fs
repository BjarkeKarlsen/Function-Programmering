open System

open System
open Akka.Actor
open Akka.FSharp
open Exam.Part2.Messages
open Exam.Part2.ParentActor
open Exam.Part2.Output
open Exam.Part2.Input


[<EntryPoint>]
let main argv =
    let actorSystem = System.create "ActorSystem" <| Configuration.defaultConfig()
    
    let strategy () = Strategy.OneForOne(( fun error ->
            match error with
            | :? ArithmeticException -> Directive.Resume
            | :? NotSupportedException -> Directive.Stop
            | _ -> Directive.Restart), 10, TimeSpan.FromSeconds(30.))
    
    let parentActor = spawnOpt actorSystem "parentActor" ( RssSubscriptionActor)
                                                [ SpawnOption.SupervisorStrategy(strategy()) ]
    
    let outputActor = spawn actorSystem "output" (actorOf outputActor)
    
    let inputActor = spawn actorSystem "input" (actorOf2 cmdActor)
    
    
    inputActor <! ContinueProcess
    
    actorSystem.WhenTerminated.Wait()
    
    
    0 
