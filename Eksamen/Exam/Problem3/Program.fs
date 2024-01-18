module Problem3.Program
open System
open FParsec
open Problem3.Parsers

[<EntryPoint>]
let main argv =
    let path = "../../../txtData.txt"
    
    let inputString = "2019-01-29T17:57:25.000Z,4957.0,40861.16"
    
    let print 
        
    match run exercisesParser inputString with
        | Success(result, _, _) -> 
            printfn "Parsed result: %A" result
        | Failure(errorMsg, _, _) ->
            
            printfn "Parsing failed: %s" errorMsg
        | _ -> failwith "todo"
    
    let input2 = "2019-01-29T17:57:25.000Z#3.740000009536743#46$3.742000102996826#77"
    match run measurementsParser input2 with
        | Success(result, _, _) -> 
            printfn "Parsed result: %A" result
        | Failure(errorMsg, _, _) ->
            
            printfn "Parsing failed: %s" errorMsg
        | _ -> failwith "todo"
    0 
