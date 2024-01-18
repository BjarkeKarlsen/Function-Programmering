module Part3.Calculates
open Part3.Type
   
let average  data =
   let total : float = data |> List.sumBy (fun x -> x.Watt)
   let amount : float = List.length data 
   total / amount
      
let powerOfFourth value =
    value ** 4.0
  
let normalized data =
    let averages = 
       data 
       |> List.skip 30
       |> List.windowed 30 
       |> List.map average
   
    let averagesToPowerOfFourth = averages |> List.map powerOfFourth
    
    let averagesOfPowerOfFourth = averagesToPowerOfFourth |> List.average
    
    let normalizedPower = averagesOfPowerOfFourth ** (1.0/4.0)
    
    normalizedPower
    
let intensityFactor (np: float) (ftp: float) =
    np / ftp
    
let trainingStressScore (duration: int) np ftp =
   ((float duration) * np * (intensityFactor np ftp)) / (ftp * 36.0)

let printExercise exercise =
    printfn $"Start Time: {exercise.StartTime}"
    printfn $"Total Time: %f{exercise.Total}"
    printfn $"Distance: %f{exercise.Distance}"

let printMeasurements measurement =
     printfn $"Time: {measurement.Time}",
     printfn   $"Distance Traveled: %f{measurement.DistanceTraveled}"
     printfn   $"Cadence: %d{measurement.Cadence}"
     printfn   $"Speed: %f{measurement.Speed}"
     printfn   $"Watt: %d{measurement.Watt}"
   
let printInfo training =
    printfn "Exercise:"
    printExercise training.Exercise
    printfn "Measurements:"
    training.Measurement |> List.iter (printMeasurements)
    
