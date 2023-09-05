module Program
open DateUtils

let year = 1970

[<EntryPoint>]
let main argv =
    let isLeap = DateUtils.isLeapYear year
    printfn $"Is leap year: %b{isLeap}"
    0