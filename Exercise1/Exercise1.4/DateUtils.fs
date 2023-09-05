module DateUtils

let epoch = 1970 

let isLeap(year: int): bool =
    ((year % 100 <> 0 && year % 4 = 0 && year > 0) || year % 400 = 0)

let daysToEndYear(year) =
    let leaps = List.filter isLeap [epoch..year] |> List.length
    ((year-epoch) * 365 + leaps)

let isJanuary(monthYear): int =
    match monthYear with
    | (m, _) when (m = 1) -> 0
    | (m, y) when (isLeap(y) && m > 1) -> 1
    | _ -> 2

let dayToEndMonth(monthYear : int * int) : int = 
    let (month, year) = monthYear
    let c = isJanuary(monthYear)
    
    (367*month+5)/12 - c + daysToEndYear (year-1) 