open FParsec
open Part3
open Part3.Type
open Calculates
open Part3.LoadFile
open Part3.Parser

let pData (data: string) : TrainingSession =
    match run pTrainingSession data with
    | Success(result, _, _) -> result
    | Failure(errorMsg, _, _) -> failwithf $"Parsing failed: %s{errorMsg}"

[<EntryPoint>]
let main argv =
    let data = file "./../../../txtData.txt"   
    match data with
    | None -> printfn "Can't load file"
    | Some text ->
        let trainingExercise = pData text          
        printInfo trainingExercise
        let np = normalized trainingExercise.Measurement
        let ftp = 250.0
        let tss = trainingStressScore (int trainingExercise.Exercise.Total) np ftp
        printfn $"Normalized Power: %f{np}"
        printfn $"Intensity Factor: %f{intensityFactor np ftp}"
        printfn $"Training Stress Score: %f{tss}"
    0