module Problem3.Parsers

open System
open FParsec
open Problem3.Type


//use to find everything until Z
let anyCharsTill pEnd = manyCharsTill anyChar pEnd

let str_ws s = pstring s .>> spaces
let char_ws c = pchar c .>> spaces
let ws_char c= spaces >>. pchar c
 

// startTime,totalTime,distance

let startTimeParser : Parser<DateTime, unit> =
    anyCharsTill (pstring ",")  |>> DateTime.Parse  
let totalTimeParser : Parser<float, unit> =
    pfloat .>> char_ws ','

let distanceParser  : Parser<float, unit> =
    pfloat .>> newline
    

let exercisesParser: Parser<DateTime * float * float, unit>=
    pipe3 startTimeParser totalTimeParser distanceParser (fun startTime totalTime distance -> (startTime, totalTime, distance))


// time#distanceTraveled#cadance
let timeParser: Parser<DateTime, unit> =
    anyCharsTill (pstring "#") |>> DateTime.Parse

let distanceTraveledParser: Parser<float, unit> =
    pfloat .>> char_ws '#'
    
let cadenceParser: Parser<int, unit> =
    pint32 .>> char_ws '$'
    

let dataPointParser: Parser<DateTime * float * int, unit> =
    pipe3 timeParser distanceTraveledParser cadenceParser (fun time distance cadence -> (time, distance, cadence))
 
// speed#watt
 
let speedParser: Parser<float, unit> =
    pfloat .>> char_ws '#'

let wattParser: Parser<int, unit> =
    pint32 .>> newline

let performanceParser: Parser<float * int, unit> =
    pipe2 speedParser wattParser (fun speed watt -> (speed, watt))
    
// time#distanceTraveled#cadance$speed#watt
let measurementsParser  =
    pipe2 dataPointParser performanceParser (fun datapoint performance -> (datapoint, performance))

let combinedParser (exercises: Exercises) (measurement: Measurement) =
    pipe2 exercisesParser (many measurementsParser) (fun exercise measurement -> (exercise, measurement))
