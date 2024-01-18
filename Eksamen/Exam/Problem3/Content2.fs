module Problem3.Content2
open Problem3.Type
open Problem3.Parsers


let average (data: Measurement list):float =
    let total = data |> List.sumBy (fun x -> x.Watt)
    let count = data |> List.length
    (float total) / (float count)
    
let calculateFourthPower (value: float) =
    value ** 4.0
       
let normalized (data: Measurement list): float =
    // Step 1: Calculate the average of the last 30 seconds for every second from 30 -> end
    let averages = 
        data 
        |> List.skip 30 // Skip the first 30 seconds
        |> List.windowed 30 // Create windows of 30 seconds
        |> List.map average // Calculate the average for each window

    // Step 2: Calculate the 4th power for every value produced in Step 1
    let fourthPowers = 
        averages 
        |> List.map calculateFourthPower

    // Step 3: Make an average of the values in Step 2
    let averageFourthPower = 
        fourthPowers 
        |> List.average

    // Step 4: Take the 4th root of the average from Step 3
    let normalizedPower = 
        averageFourthPower ** (1.0/4.0)
        
    normalizedPower
    
let intensityFactor (np: float) (ftp: float) : float =
    np / ftp

let trainingStressScore (duration: int) (np: float) (ifValue: float) (ftp: float) : float =
    let directionInSeconds = float duration
    (directionInSeconds * np * ifValue) / (ftp * 36.0)
