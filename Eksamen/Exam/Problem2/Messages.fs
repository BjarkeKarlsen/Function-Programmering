// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
module Problem2.Messages

type ConsoleWriterMessages =

    | Menu of string
    | ConsoleLine of string
    | Success of string
    | Error of string


    
type RssSubscriptionMessages =
    | Start
    | Subscribe of string
    | Unsubscribe of string
    | Refresh of string
    | RefreshAll
    | GetAggregatedFeed
    | Stop
    
type CmdMessages =
    | CreateProcessing
    | StartProcessing
    | ExitProcessing
    
    
type RssFeedMessages =
    | CreateFetcher of string
    | Refresh of string
    | GetData of string
    | Cancel
    
    
type FetcherMessages =
    | Fetch