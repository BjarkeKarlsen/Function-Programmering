module Problem3.Program
open System
open System.IO
open FParsec
open Problem3.Parsers
open Problem3.Type
open Problem3.Calculations

[<EntryPoint>]
let main argv =
    let data = File.ReadAllLines("../../../txtData.txt")
    
    let exercises, measurements = parseData(String.concat "\n" data)
    
    
    let normalized = normalized measurements
    let ftp = 300
    let intensityFactor = intensityFactor normalized ftp
    let tss = trainingStressScore exercises.TotalTime normalized intensityFactor ftp
    
    printfn $"Normalized data: {normalized}"
    printfn $"Intensity Factor: {intensityFactor}"
    printfn $"Intensity Factor: {tss}"
    print exercises measurements

    0 
