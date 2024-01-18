module Problem2.RssFeedActor
//
// open System
// open Akka.Actor
// open Akka.FSharp
// open Problem2.Messages
//
//
// //with mutable data
//
//     
// // Without mutable
//
// let ChildActor (mailbox:Actor<ChildMessages>) msg=
//      
//     // TODO: Logic outside of loop
//     
//     let output status message =
//          select "/user/output" mailbox.Context.System <! status message
//                      
//     match msg with
//     | ChildMessages.Start -> ()// TODO: Startprocess
//     | Cancel ->
//                 mailbox.Context.Stop(mailbox.Self)
//    
//     
//                 
//
//     
//         
//     
//      
//     
//     
