module Part3.Parser

open System
open FParsec
open Microsoft.FSharp.Core
open Part3.Type

let str_ws s = pstring s .>> spaces
let char_ws c = pchar c .>> spaces
let ws_char c = spaces >>. pchar c
let anyCharsTill pEnd = manyCharsTill anyChar pEnd

let pDateTime : Parser<DateTime, unit> =
    anyCharsTill (pchar ',' <|> pchar '#') |>> DateTime.Parse

let pTotalTime : Parser<float, unit> =
    pfloat .>> char_ws ','

let pDistance : Parser<float, unit> =
    pfloat .>> newline
    
let pExercise : Parser<Exercise, unit> =
    pipe3 pDateTime pTotalTime pDistance (fun dateTime totalTime distance -> { StartTime = dateTime; Total = totalTime; Distance=distance })

let pTime : Parser<DateTime, unit> =
    anyCharsTill (pchar '#') |>> DateTime.Parse
    
let PDistanceTraveled : Parser<float, unit> =
    pfloat .>> char_ws '#'

let PCadence : Parser<int, unit> =
    pint32 .>> char_ws '$'
    
let pSpeed : Parser<float, unit> =
    pfloat .>> char_ws '#'

let pWatt : Parser<int, unit> =
    pint32 .>> newline


let pMeasurement : Parser<Measurement, unit> =
    pipe5 pTime PDistanceTraveled PCadence pSpeed pWatt
        (fun time distanceTraveled cadence speed watt -> { Time = time; DistanceTraveled = distanceTraveled; Cadence = cadence; Speed = speed; Watt = watt})

let pTrainingSession : Parser<TrainingSession, unit> = 
    pipe2 pExercise (many pMeasurement) (fun exercise measurement -> {Exercise = exercise; Measurement= measurement })
