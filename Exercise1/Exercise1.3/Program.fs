module Program
open System
let isLeapYear(year: int): bool =
    ((year % 100 <> 0 && year % 4 = 0 && year > 0) || year % 400 = 0)

let daysToEndYear(year: int): int =
    (year*365 + (1 * (abs(year - 1972) /4)%1))
       
        
[<EntryPoint>]
let main argv =
    printfn "Enter a year:"
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | true, year ->
        let isLeap = isLeapYear year
        let epochDay = daysToEndYear year
        printfn "Is leap year: %b" isLeap
        printfn "Epoch-day for 31/12/%d is %d" year epochDay
        0
    | _ ->
        printfn"Invalid input. Please enter a valid year"
        1