module Problem3.Parsers

open System
open FParsec
open Problem3.Type


let anyCharsTill pEnd = manyCharsTill anyChar pEnd

let char_ws c = pchar c .>> spaces


let startTimeParser : Parser<DateTime, unit> =
    anyCharsTill (pstring ",")  |>> DateTime.Parse  
let totalTimeParser : Parser<float, unit> =
    pfloat .>> char_ws ','

let distanceParser  : Parser<float, unit> =
    pfloat .>> spaces
    

let exercisesParser: Parser<Exercises, unit>=
    pipe3 startTimeParser totalTimeParser distanceParser (fun startTime totalTime distance -> {
        StartTime = startTime; TotalTime = totalTime; Distance = distance
    })


let timeParser: Parser<DateTime, unit> =
    anyCharsTill (pstring "#") |>> DateTime.Parse

let distanceTraveledParser: Parser<float, unit> =
    pfloat .>> char_ws '#'
    
let cadenceParser: Parser<int, unit> =
    pint32 .>> char_ws '$'
    
let speedParser: Parser<float, unit> =
    pfloat .>> char_ws '#'

let wattParser: Parser<int, unit> =
    pint32 .>> spaces
let measurementsParser: Parser<Measurement, unit> =
    pipe5 timeParser distanceTraveledParser cadenceParser speedParser wattParser (fun time distance cadence speed watt-> {
        Measurement.Time = time;
        Measurement.DistanceTraveled = distance;
        Measurement.Cadence = cadence;
        Measurement.Speed = speed;
        Measurement.Watt = watt 
    })

let combinedParser=
    pipe2 exercisesParser (many measurementsParser) (fun exercise measurement -> (exercise, measurement))
