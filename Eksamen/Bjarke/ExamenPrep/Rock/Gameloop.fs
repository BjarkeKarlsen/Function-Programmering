module ConsoleApp1.Gameloop 

open System
open GameInfo
open Weapon
open ConsoleGUI

let outCome (weapon1, weapon2) =
    match weapon1, weapon2 with
    | Rock, Scissors | Rock, Lizard -> Win
    | Scissors, Paper | Scissors, Lizard -> Win
    | Paper, Rock | Paper, Spock -> Win
    | Lizard, Spock | Lizard, Paper -> Win
    | Spock, Rock | Spock, Scissors -> Win
    | weapon1, weapon2 when weapon1 = weapon2 -> Tie
    | _ -> Lose

type Action =
    | PlayRound
    | MaxRounds
    | Quit

let tryToInt (input: string) =
    match Int32.TryParse(input) with
    | (true, value) -> Some value
    | _ -> None
    
type PlayerAction =
    | Some of Weapon
    | None

let rec action () =
    let player = Console.ReadLine()
    match player with
    | "q" -> None
    | _ when tryToInt player |> Option.isSome ->
        Some ((getWeaponFromNumber (tryToInt player |> Option.get)))
    | _ ->
        printfn "Enter a valid number between 1 and 5"
        action ()

let updateAction total =
    let maxRounds = 10

    match total with
    | value when value >= maxRounds -> MaxRounds
    | _ -> PlayRound
    

let playRound scoreboard =
    let playerAction = action()
    
    match playerAction with
    | None -> Quit
    | Some weapon ->
        let computer = generateChoice |> aiPicksGUI 
        outCome(weapon, computer)
        |> roundEndedGUI
        |> (fun newScore -> updateScore newScore scoreboard)
        |> (fun score -> updateAction score.Total)

let run =     
    let mutable scoreboard = { Player = 0; Computer = 0; Total = 0 }
    let action = Action.PlayRound;  
   
    playerChoiceGUI
    
    let rec gameLoop action =
        match action with
        | PlayRound -> 
            playRound scoreboard 
            |> gameLoop
        | MaxRounds | Quit -> 
            scoreGUI scoreboard (calculatePercentage scoreboard)
            |> (fun _ -> 0)

    gameLoop action
    0

