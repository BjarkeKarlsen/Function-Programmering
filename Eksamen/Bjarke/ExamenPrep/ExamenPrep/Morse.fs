module ExamenPrep.Morse

type Morse =
    | Dot
    | Dash

let charToMorse = function
    | 'a' -> [ Dot; Dash ]
    | 'b' -> [ Dash; Dot; Dot; Dot ]
    | 'c' -> [ Dash; Dot; Dash; Dot ]
    | 'd' -> [ Dash; Dot; Dot ]
    | 'e' -> [ Dot ]
    | 'f' -> [ Dot; Dot; Dash; Dot ]
    | 'g' -> [ Dash; Dash; Dot ]
    | 'h' -> [ Dot; Dot; Dot; Dot ]
    | 'i' -> [ Dot; Dot ]
    | 'j' -> [ Dot; Dash; Dash; Dash ]
    | 'k' -> [ Dash; Dot; Dash ]
    | 'l' -> [ Dot; Dash; Dot; Dot ]
    | 'm' -> [ Dash; Dash ]
    | 'n' -> [ Dash; Dot ]
    | 'o' -> [ Dash; Dash; Dash ]
    | 'p' -> [ Dot; Dash; Dash; Dot ]
    | 'q' -> [ Dash; Dash; Dot; Dash ]
    | 'r' -> [ Dot; Dash; Dot ]
    | 's' -> [ Dot; Dot; Dot ]
    | 't' -> [ Dash ]
    | 'u' -> [ Dot; Dot; Dash ]
    | 'v' -> [ Dot; Dot; Dot; Dash ]
    | 'w' -> [ Dot; Dash; Dash ]
    | 'x' -> [ Dash; Dot; Dot; Dash ]
    | 'y' -> [ Dash; Dot; Dash; Dash ]
    | 'z' -> [ Dash; Dash; Dot; Dot ]
    | _ -> [] // Handle other characters as needed

let stringToMorse (text: string) =
    text.ToLower()
    |> Seq.map charToMorse
    |> Seq.toList

let convertToSymbol = function
    | Dot -> "."
    | Dash -> "-"

let morseToSymbol morse =
    morse
    |> List.map (fun charMorse ->
        charMorse
        |> List.map convertToSymbol
        |> String.concat ""
    )
    |> String.concat ""

let smorse text = 
    text |> stringToMorse |> morseToSymbol
    
let countSymbols (text : string) =
    text
    |> Seq.fold (fun (dotCount, dashCount) symbol ->
        match symbol with
        | '.' -> (dotCount + 1, dashCount)
        | '-' -> (dotCount, dashCount + 1)
        | _ -> (dotCount, dashCount)
    ) (0, 0)

let splitWords (words: string) =
    words.Split([|' '; '\n'; '\r'; '\t'|]) |> Seq.toList
    
let findMostRepeatedMorse list =
    list
    |> List.groupBy id
    |> List.maxBy (fun (_, group) -> List.length group)
    |> (fun (morse, group) -> morse, List.length group)
    
    
let getMorseAndLetterList text =
    let words = splitWords text
    let morses = List.map smorse words
    let (mostRepeatedMorse, _) = findMostRepeatedMorse morses
    let dict = List.zip words morses
    dict |> List.choose (fun (word, morse) -> if morse = mostRepeatedMorse then Some word else None)

let printListElements (list: string list) =
    list |> List.iter (fun element -> printfn $"%s{element}")  

