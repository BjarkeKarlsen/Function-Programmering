    let epoch = 1970
    
    let isLeap = function
        | year when (year % 400) = 0 -> true
        | year when (year % 100 <> 0 && year % 4 = 0) -> true
        | _ -> false
    
    let daysToEndYear year =
        let leapYears = List.filter isLeap [epoch..year] 
        (year-epoch)*365 + (List.length leapYears)
    
    let daysToEndMonth (month, year) =
        let correction = match month with
                         | 1 -> 0
                         | _ when isLeap year -> 1
                         | _ -> 2
        (367*month+5)/12 - correction + daysToEndYear (year-1)
        
    let epochDays (day, month, year) =
        day + daysToEndMonth (month-1, year) 


let occFromIth (str: string, i, ch) =
    match i with
    | index when index < 0 || index >= str.Length -> 0
    | index -> str.Substring index 
            |> Seq.filter(fun charAtIndex -> charAtIndex = ch)
            |> Seq.length

let c = occFromIth("sdddad", 2, 'd')

printf "%d" c