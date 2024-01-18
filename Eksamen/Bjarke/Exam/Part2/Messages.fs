// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
module Exam.Part2.Messages

open System

type Item = {
    Title: string
    Description: string
    Link: string
    Guid: string
    PubDate: DateTime
}

type ConsoleWriterMessages =
    | Menu of string
    | ConsoleLine of string
    | Success of string
    | ValidationError of string
    | NullInputError of string

type RssFeedMessages =
    | Refresh
    | GetData of AsyncReplyChannel<Item list>
    | Reply of string
    
type RssSubscriptionMessages =
    | Subscribe of string
    | UnSubscribe of string
    | Refresh of string
    | RefreshAll
    | GetAggregatedFeed

type FetcherMessages =
    | Fetch of AsyncReplyChannel<Item list>

type InputMessages =
    | Subscribe 
    | UnSubscribe 
    | Refresh 
    | RefreshAll
    | GetAggregatedFeed
    | ContinueProcess
    | Exit