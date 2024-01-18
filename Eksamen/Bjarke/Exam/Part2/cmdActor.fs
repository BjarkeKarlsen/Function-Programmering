module Exam.Part2.Input

open System
open Akka.FSharp
open Exam.Part2.Messages

let (|ToInputMessage|_|) (cmdStr: string) =
    match cmdStr.ToLower() with
    | "subscribe" -> Some Subscribe
    | "unsubscribe" -> Some UnSubscribe
    | "refresh" -> Some Refresh
    | "refreshall" -> Some RefreshAll
    | "getaggregatedfeed" -> Some GetAggregatedFeed
    | _ -> None

let cmdActor (mailbox: Actor<InputMessages>) msg =
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    match msg with
    | StartProcessing ->
        output Menu "Please enter command as: [command:argument]"
        output Menu "Subscribe to url"
        output Menu "UnSubscribe to url"
        output Menu "Refresh url"
        output Menu "Refresh all"
        output Menu "Get Aggregated Feed"
        output Menu "Exit"
        
        let command = Console.ReadLine().ToLower().Split(':')
        match command with
        | [| ""; _ |] ->
            output ValidationError "Error: command missing"
        | [| "exit" |] -> 
            printf "Exit" 
        | [| (ToInputMessage cmd); arg |] ->
            match cmd with
            | Subscribe -> select "/user/parentActor" mailbox.Context.System <! RssSubscriptionMessages.Subscribe arg
            | UnSubscribe -> select "/user/parentActor" mailbox.Context.System <! RssSubscriptionMessages.UnSubscribe arg
            | Refresh -> select "/user/parentActor" mailbox.Context.System <! RssSubscriptionMessages.Refresh arg
            | RefreshAll -> select "/user/parentActor" mailbox.Context.System <! RssSubscriptionMessages.RefreshAll
            | GetAggregatedFeed -> select "/user/parentActor" mailbox.Context.System <! RssSubscriptionMessages.GetAggregatedFeed
            | Exit -> output ValidationError "Error: unknown command"
            | _ -> output ValidationError "Error: invalid command format"
        | _ -> output ValidationError "Error: invalid command format"

    Threading.Thread.Sleep(1000)
    mailbox.Self <! ContinueProcess