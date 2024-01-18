module Problem2.CmdActorIdle
// open System
// open Akka.Actor
// open Akka.FSharp
// open Problem2.Messages
//
// let tryToInt (input: string) =
//     match Int32.TryParse(input) with
//     | (true, value) -> Some value
//     | _ -> None
//
//
//
// let inputActor (mailbox: Actor<CmdMessages>) msg =
//     let output status message =
//          select "/user/output" mailbox.Context.System <! status message
//          
//     match msg with
//     | StartProcessing ->
//         output Menu "Press 1 to subscribe to url"
//         output Menu "Press 2 to subscribe to url "
//         output Menu "Press 3 to refresh url"
//         output Menu "Press 4 to refresh all"
//         output Menu "Press 5 Get aggregated RSS Feed with data from all RSS Feeds"
//         output Menu "Press 6 to Exit"
//         
//         let line = Console.ReadLine().ToLower()
//         
//         match line with 
//         | "1" ->
//             
//         | "2" -> 
//             mailbox.Self <! CreateProcessing
//         | "3" ->
//         |"4" ->
//         | "5"
//            
//             mailbox.Self <! ExitProcessing
//         | "6" ->
//             mailbox.Context.Stop(mailbox.Self) |> ignore
//         | _ ->
//             output Error "Please enter something valid"
//             mailbox.Self <! StartProcessing
//     | CreateProcessing ->
//         output ConsoleLine "Enter a ticket seller you want to create"
//         let line = Console.ReadLine()
//         match line with
//         | (line) when String.IsNullOrEmpty line ->
//             output Error "Not valid"
//             mailbox.Self <! StartProcessing
//         | (line)  ->
//             select "/user/parentActor" mailbox.Context.System <! CreateChild line
//             mailbox.Self <! StartProcessing
//        
//     | ExitProcessing -> failwith "todo"
//     | _ -> output Error "oopsies :)"    