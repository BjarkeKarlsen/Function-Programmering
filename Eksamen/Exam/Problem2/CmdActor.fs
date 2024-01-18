module Problem2.CmdActor
open System
open Akka
open Akka.Actor
open Akka.FSharp.Actors
open Problem2.Messages



let tryToInt (input: string) =
    match Int32.TryParse(input) with
    | (true, value) -> Some value
    | _ -> None



let CmdActor (mailbox: Actor<CmdMessages>) msg =
    let output status message =
         select "/user/output" mailbox.Context.System <! status message
         
    match msg with
    | StartProcessing ->
        output Menu "Press 1 to subscribe to url"
        output Menu "Press 2 to unsubscribe to url "
        output Menu "Press 3 to refresh url"
        output Menu "Press 4 to refresh all"
        output Menu "Press 5 Get aggregated RSS Feed with data from all RSS Feeds"
        output Menu "Press 6 to Exit"
        
        let line = Console.ReadLine().ToLower()
        
        match line with 
        | "1" ->
            output ConsoleLine  "Write an URL you want to subscribe to"
            let name = Console.ReadLine()
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Subscribe name 
            mailbox.Self <! StartProcessing

        | "2" ->
            output ConsoleLine "Write an URL you want to unsubscribe to"
            let name = Console.ReadLine()

            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Unsubscribe name

            mailbox.Self <! StartProcessing
        | "3" ->
            output ConsoleLine "Write an URL you want to refresh"
            let name = Console.ReadLine()

            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.Refresh name
            mailbox.Self <! StartProcessing

        |"4" ->
            output ConsoleLine "Refreshing all"
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.RefreshAll
            mailbox.Self <! StartProcessing
        | "5" ->
            output ConsoleLine "Get all data"
            select "/user/subscriptionActor" mailbox.Context.System <! RssSubscriptionMessages.GetAggregatedFeed
            mailbox.Self <! StartProcessing
        | "6" ->
            output ConsoleLine "Exiting..."
            mailbox.Context.Stop(mailbox.Self) |> ignore
        | _ ->
            output Error "Please enter something valid"
            mailbox.Self <! StartProcessing
    | _  -> output Error "Not started"
       