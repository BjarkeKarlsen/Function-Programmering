module Problem3.Calculations
open Problem3.Type
open Problem3.Parsers
open FParsec

let average (data: Measurement list):float =
    let total = data |> List.sumBy (fun x -> x.Watt)
    let count = data |> List.length
    (float total) / (float count)
    
let calculateFourthPower (value: float) =
    value ** 4.0
       
let normalized (data: Measurement list): float =
    let averages = 
        data 
        |> List.skip 30 
        |> List.windowed 30 
        |> List.map average 

    
    let fourthPowers = 
        averages 
        |> List.map calculateFourthPower

    let averageFourthPower = 
        fourthPowers 
        |> List.average

    let normalizedPower = 
        averageFourthPower ** (1.0/4.0)
        
    normalizedPower
    
let intensityFactor (np: float) (ftp: float) : float =
    np / ftp

let trainingStressScore (duration: float) (np: float) (ifValue: float) (ftp: float) : float =
    let directionInSeconds = float duration
    (directionInSeconds * np * ifValue) / (ftp * 36.0)

let print (exercises: Exercises) (measurements: Measurement list) =
        printfn "Exercises:"
        printfn $"StartTime: {exercises.StartTime}"
        printfn $"TotalTime: {exercises.TotalTime}"
        printfn $"Distance: {exercises.Distance}"
        printfn "Measurements:"
        measurements |> List.iter (fun m ->
            printfn $"Time: {m.Time}"
            printfn $"DistanceTraveled: {m.DistanceTraveled}"
            printfn $"Speed: {m.Speed}"
            printfn $"Cadence: {m.Cadence}"
            printfn $"Watt: {m.Watt}")
    

    
let parseData (data: string) =
    match run combinedParser data with
        | Success(result, _, _) -> result
        | Failure(errorMsg, _, _) -> failwith errorMsg
        | _ -> failwith "todo"
        