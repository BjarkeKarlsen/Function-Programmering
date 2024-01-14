module Exam1.Program
open Exam1.FetchStrings

// For more information see https://aka.ms/fsharp-console-apps
// type Morse = A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|X|Y|Z

// printfn "Hello from F#"

[<EntryPoint>]

    let main _ =
        
        
        let url = "https://raw.githubusercontent.com/dolph/dictionary/master/enable1.txt"
        let data = downloadFileAsync(url)
        let output = smorse data
        let (dots, dashes) = countSymbols output
        printf $"{output} dot counts: {dots} dash counts {dashes} "
        let (mostSequence, length, morse) = findSequenceForWords data
        printfn $"Sequences that correspond to 13 different words: {morse} with the length {length} with the words: {mostSequence}"

        0