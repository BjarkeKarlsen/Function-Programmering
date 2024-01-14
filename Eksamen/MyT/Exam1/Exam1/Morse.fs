module Exam1.Morse

type Morse =
    | Dot
    | Dash
    | Error

let charToMorse = function
    | 'a' -> [Dot; Dash]
    | 'b' -> [Dash; Dot; Dot; Dot]
    | 'c' -> [Dash; Dot; Dash; Dot]
    | 'd' -> [Dash; Dot; Dot]
    | 'e' -> [Dot]
    | 'f' -> [Dot; Dot; Dash; Dot]
    | 'g' -> [Dash; Dash; Dot]
    | 'h' -> [Dot; Dot; Dot; Dot]
    | 'i' -> [Dot; Dot]
    | 'j' -> [Dot; Dash; Dash; Dash]
    | 'k' -> [Dash; Dot; Dash]
    | 'l' -> [Dot; Dash; Dot; Dot]
    | 'm' -> [Dash; Dash]
    | 'n' -> [Dash; Dot]
    | 'o' -> [Dash; Dash; Dash]
    | 'p' -> [Dot; Dash; Dash; Dot]
    | 'q' -> [Dash; Dash; Dot; Dash]
    | 'r' -> [Dot; Dash; Dot]
    | 's' -> [Dot; Dot; Dot]
    | 't' -> [Dash]
    | 'u' -> [Dot; Dot; Dash]
    | 'v' -> [Dot; Dot; Dot; Dash]
    | 'w' -> [Dot; Dash; Dash]
    | 'x' -> [Dash; Dot; Dot; Dash]
    | 'y' -> [Dash; Dot; Dash; Dash]
    | 'z' -> [Dash; Dash; Dot; Dot]
    | _ -> [Error] // Use CharSeparator for unrecognized characters
                               
    
let convertLettertoType (input: string) =
    input.ToLower()
    |> Seq.collect charToMorse
    |> Seq.toList
    
let convertTypetoSymbol = function       
    | Dot -> "."                         
    | Dash -> "-"
    | Error -> ""

let typeToSymbol (morse: Morse list) =
    morse
    |> List.map (fun s -> convertTypetoSymbol s )
    |> String.concat ""
       
let stringToType (input: string) =
    input.ToLower()
    |> Seq.collect charToMorse
    |> Seq.toList

let smorse text =
    text |> stringToType |> typeToSymbol
    
let countSymbols (text:string) =
    text |> Seq.fold (fun (dotCount, dashCount) symbol ->
        match symbol with
        | '.' -> (dotCount+1, dashCount)
        | '-' -> (dotCount, dashCount+1)
        | _ -> (dotCount, dashCount)
        ) (0,0)
  
   
let wordSplit (word: string) =
        word.Split([|'\n'; '\r'|]) |> Seq.toList

let findMostRepeatedMorse list =
    list
    |> List.groupBy id
    |> List.maxBy (fun (_, group) -> List.length group)
    |> (fun (morse, group ) -> morse, List.length group)
 
let findSequenceForWords data =
    let words = wordSplit data
    let wordsinmorse = List.map smorse words
    let combinedlist = List.zip words wordsinmorse
    let (mostRepeatedMorse, count) = findMostRepeatedMorse wordsinmorse
    let repeatedWords = combinedlist |> List.choose(fun (word, morse) -> if morse = mostRepeatedMorse then Some word else None)
    repeatedWords, count, mostRepeatedMorse
    
    