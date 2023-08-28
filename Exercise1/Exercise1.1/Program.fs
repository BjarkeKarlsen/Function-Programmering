let isLeap(year: int) = ((not (year % 100 = 0) && year % 4 = 0) || year % 400 = 0  )
let dispBool(value: bool) = if(value) then printfn "true" else printfn "false"

let leap = isLeap(1972)
dispBool(leap)
let leap1 = isLeap(1992)
dispBool(leap1)

let leap2000 = isLeap(2000)
dispBool(leap2000)

let leap1900 = isLeap(1900)
dispBool(leap1900)