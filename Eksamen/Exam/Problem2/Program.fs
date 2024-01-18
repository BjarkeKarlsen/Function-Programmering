module Part2.Program

open System
open Akka.Actor
open Akka.FSharp
open Problem2
open Problem2.Messages
open Problem2.RssFetcherActor
open Problem2.RssFetcherActor
open Problem2.RssSubscriptionActor
open Problem2.CmdActor
open Problem2.RssFeedActor
open Problem2.Output


[<EntryPoint>]
let main argv =
    let actorSystem = System.create "ActorSystem" <| Configuration.defaultConfig()
    
    let strategy () = Strategy.OneForOne(( fun error ->
            match error with
            | :? ArithmeticException -> Directive.Resume
            | :? NotSupportedException -> Directive.Stop
            | _ -> Directive.Restart), 10, TimeSpan.FromSeconds(30.))
    
    let rssSubscriptionActor = spawnOpt actorSystem "subscriptionActor" ( RssSubscriptionActor)
                                                [ SpawnOption.SupervisorStrategy(strategy()) ]
    
    let outputActor = spawn actorSystem "output" (actorOf OutputActor)
    
    let cmdActor = spawn actorSystem "input" (actorOf2 CmdActor)
    
    
    cmdActor <! StartProcessing
    
    actorSystem.WhenTerminated.Wait()
    
    
    0 
