open ExamenPrep.Morse
open ExamenPrep.FetchStrings

module Program =
    
    [<EntryPoint>]
    let main _ =
        let data = getUrlAsync("https://raw.githubusercontent.com/dolph/dictionary/master/enable1.txt")
        
        let smore = smorse data
        let (dots, dashes) = countSymbols smore
        let words = getMorseAndLetterList data
        
        printfn $"{smore}"
        printfn $"Dots {dots} and Dash {dashes}"
        printListElements words
        0