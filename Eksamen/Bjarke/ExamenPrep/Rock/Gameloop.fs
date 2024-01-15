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
    

let playRound scoreboard random aiState =
    let playerAction = action()
    
    match playerAction with
    | None -> Quit, scoreboard
    | Some weapon ->
        let computer = generateChoice random |> aiPicksGUI 
        let newScoreboard = 
            outCome(weapon, computer)
            |> roundEndedGUI
            |> (fun newScore -> updateScore newScore scoreboard)

        let newAction = updateAction newScoreboard.Total
        newAction, newScoreboard

let run =     
    let action = Action.PlayRound
    let random = Random()
    let aiState = initAIState
   
    playerChoiceGUI
    
    let rec gameLoop action scoreboard =
        match action with
        | PlayRound -> 
            let newAction, newScoreboard = playRound scoreboard random 
            gameLoop newAction newScoreboard aiState
        | MaxRounds | Quit -> 
            scoreGUI scoreboard (calculatePercentage scoreboard)
            |> (fun _ -> 0)

    gameLoop action { Player = 0; Computer = 0; Total = 0 }, aiState
    0

