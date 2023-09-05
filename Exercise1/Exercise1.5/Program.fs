module Program
open System
open DateUtils

[<EntryPoint>]
let main argv =
    printfn "Enter a month:"
    let inputMonth = Console.ReadLine()
    
    printfn "Enter a year:"
    let input = Console.ReadLine()
   
    match Int32.TryParse(inputMonth) with
    | true, month ->
        match Int32.TryParse(input) with
        | true, year ->
            let isLeap = isLeapYear year
            let dayToEndMonth = dayToEndMonth (month, year) 
            printfn $"Is leap year: %b{isLeap}"
            printfn $"The month and year: %d{dayToEndMonth}"
            0
        | _ ->
            printfn"Invalid input. Please enter a valid year"
            1
    | _ ->
            printfn"Invalid input. Please enter a month"
            1