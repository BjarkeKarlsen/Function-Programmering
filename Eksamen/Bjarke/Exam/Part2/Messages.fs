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
    
type ParentMessages =
    | Start
    | CreateChild of string
    | CallChild of string
    | Stop
    
type InputMessages =
    | CreateProcessing
    | StartProcessing
    | ExitProcessing 