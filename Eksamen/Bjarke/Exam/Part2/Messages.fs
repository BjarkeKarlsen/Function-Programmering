﻿// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
module Exam.Part2.Messages

type ConsoleWriterMessages =
    | Menu of string
    | ConsoleLine of string
    | Success of string
    | ValidationError of string
    | NullInputError of string

type RssFeedMessages =
    | Refresh
    | GetData
    
type RssSubscriptionMessages =
    | Subscribe of string
    | UnSubscribe of string
    | Refresh of string
    | RefreshAll
    | GetAggregatedFeed

type FetcherMessages =
    | Fetch

type InputMessages =
    | Subscribe 
    | UnSubscribe 
    | Refresh 
    | RefreshAll
    | GetAggregatedFeed
    | ContinueProcess
    | Exit