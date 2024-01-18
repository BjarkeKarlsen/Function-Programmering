// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
module Problem2.Messages

type ConsoleWriterMessages =

    | Menu of string
    | ConsoleLine of string
    | Success of string
    | Error of string


    
type RssSubscriptionMessages =
    | Subscribe of string
    | Unsubscribe of string
    | Refresh of string
    | RefreshAll
    | GetAggregatedFeed
    
type CmdMessages =
    | CreateProcessing
    | StartProcessing
    | ExitProcessing
    
    
type RssFeedMessages =
    | Refresh 
    | GetData
    | PoisonPill
  

type RssFeedState =
    | Fresh of string  
    | Fetching
    
type FetcherMessages =
    | Fetch