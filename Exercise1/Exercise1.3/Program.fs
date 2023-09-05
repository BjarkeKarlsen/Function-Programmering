module Program
open System

let epoch = 1970
let isLeap(year) =
    ((year % 100 <> 0 && year % 4 = 0 && year > 0) || year % 400 = 0)
  
let daysToEndYear(year) =
    let leaps = List.filter isLeap [epoch..year] |> List.length
    ((year-epoch) * 365 + (leaps))
       
        
[<EntryPoint>]
let main argv =
    printfn "Enter a year:"
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | true, year ->
        let isLeap = isLeap year
        let epochDay = daysToEndYear year
        printfn "Is leap year: %b" isLeap
        printfn "Epoch-day for 31/12/%d is %d" year epochDay
        0
    | _ ->
        printfn"Invalid input. Please enter a valid year"
        1