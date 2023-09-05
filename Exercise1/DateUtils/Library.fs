namespace DateUtils

module DateUtils =
    
    let isLeapYear(year: int): bool =
        ((year % 100 <> 0 && year % 4 = 0 && year > 0) || year % 400 = 0)

    let daysToEndYear(year: int): int =
        (year*365 + (1 * (abs(year - 1972) /4)%1))
        
    let isJanuary(monthYear : int * int): int =
        match monthYear with
        | (m, _) when (m = 1) -> 0
        | (m, y) when (isLeapYear(y) && m > 1) -> 1
        | _ -> 2
            

    let dayToEndMonth(monthYear : int * int) : int = 
        let (month, _) = monthYear
        let c = isJanuary(monthYear)
        
        ((367*month+5)/12)-c  

    let epochDay(date: int*int*int) : int =
        let (day1, day3, day2 ) = date
        day1
       