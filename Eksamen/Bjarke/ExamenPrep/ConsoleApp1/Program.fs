
open System
open Microsoft.FSharp.Reflection

type Weapon =
    | Rock
    | Paper
    | Scissors
    | Lizard
    | Spock
    
type OutCome =
    | Player
    | Computer
    | Tie

type Score =  { Player: int; Computer: int }

let getCases<'T> () =
    typeof<'T>
    |> FSharpType.GetUnionCases
    |> Array.map (fun caseInfo -> FSharpValue.MakeUnion(caseInfo, [||]))

    
let outCome (weapon1, weapon2) =
    match weapon1, weapon2 with
    | Rock, Scissors | Rock, Lizard -> Player
    | Scissors, Paper | Scissors, Lizard -> Player
    | Paper, Rock | Paper, Spock -> Player
    | Lizard, Spock | Lizard, Paper -> Player
    | Spock, Rock | Spock, Scissors -> Player
    | weapon1, weapon2 when weapon1 = weapon2 -> Tie
    | _ -> Computer
    
let roundEnded outCome = 
    match outCome with
    | Player -> printfn "Player Won this round"
    | Computer -> printfn "Computer won this round"
    | Tie -> printfn "It was a tie"
    outCome

let updateScore point score =
    match point with
    | Player -> (score.Player + 1, score.Computer)
    | Computer -> (score.Player, score.Computer + 1)
    | Tie -> (score.Player, score.Computer)

let generateChoice =
    let  weapons : Weapon = getCases<Weapon>()
                                 |> Array.map unbox
                                 |> Array.toList
                                 |> (fun weapons -> weapons.[Random().Next(weapons.Length)])
    weapons
 
let getWeaponFromNumber index =
    let weapons : Weapon list = getCases<Weapon>() |> Array.map unbox |> Array.toList
    match List.tryItem index weapons with
    | Some weapon -> weapon
    | None -> failwithf "Invalid input"
    
let playerChoice =
    printfn "Choose a weapon:"
    printfn "1. Rock"
    printfn "2. Paper"
    printfn "3. Scissors"
    printfn "4. Lizard"
    printfn "5. Spock"
    
    let playerInput = Console.ReadLine()
    getWeaponFromNumber (int playerInput)
    
    

let playerPicks value =
    $"Player picks: {value}."

let aiPicks value =
    $"Computer picks: {value}."

let gameScore (scoreboard) =
    printfn $"Player score: %d{scoreboard.Player} and Computer score: %d{scoreboard.Computer} "
    
let gameEnded (scoreboard, maxScore) =
    match scoreboard with
        | { Player = p; Computer = c } when p >= maxScore -> printfn "Player wins!"
        | { Player = p; Computer = c } when c >= maxScore -> printfn "Computer wins!"
        | _ -> printfn "Game tied!"

[<EntryPoint>]
    let main _ =
        // Initial paremeters
        let scoreboard = { Player = 0; Computer = 0 }
        let maxScore = 10
        
        
        // Missing game loop
        
        // Flow
        let player = playerChoice
        let computer = generateChoice
        printfn $"%s{playerPicks player}"
        printfn $"%s{aiPicks computer}"
        
        let score =  outCome(player, computer) |> roundEnded |> (fun x -> updateScore x scoreboard) 
        
        // Game end
        
        0