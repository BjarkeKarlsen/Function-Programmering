module Program
open System
let isLeapYear(year: int): bool =
    ((year % 100 <> 0 && year % 4 = 0 && year > 0) || year % 400 = 0)

let daysToEndYear(year: int): int =
    (year*365 + (1 * (abs(year - 1972) /4)%1))
    
let isJanuary(m : int, y : int): int =
    if (m = 1) then
        0
    else if (isLeapYear(y) && m > 1) then
        1
    else
        0

let dayToEndMonth(monthYear : int * int) : int = 
    let (month, _) = monthYear
    let c = isJanuary(monthYear)
    
    ((367*month+5)/12)-c  

let epochDay(date: int*int*int) : int =
    let (day1, day3, day2 ) = date
    day1
        
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
            printfn "Is leap year: %b" isLeap
            printfn "The month and year: %d" dayToEndMonth
            0
        | _ ->
            printfn"Invalid input. Please enter a valid year"
            1
    | _ ->
            printfn"Invalid input. Please enter a month"
            1