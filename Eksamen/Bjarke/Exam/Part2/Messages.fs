// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
module Exam.Part2.Messages

type ConsoleWriterMessages =
    | Menu of string
    | ConsoleLine of string
    | Success of string
    | ValidationError of string
    | NullInputError of string

type ChildMessages =
    | Start
    | Cancel
    
type RssSubscriptionMessages =
    | Subscribe of string
    | UnSubscribe of string
    | Refresh of string
    | RefreshAll
    | GetAggregatedFeed
    
type InputMessages =
    | Subscribe 
    | UnSubscribe 
    | Refresh 
    | RefreshAll
    | GetAggregatedFeed
    | ContinueProcess
    | Exit