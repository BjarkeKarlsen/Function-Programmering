module ConsoleApp1.ConsoleGUI

open System
open ConsoleApp1.Weapon
open ConsoleApp1.GameInfo

let playerChoiceGUI =
    printfn "Choose a weapon:"
    printfn "1. Rock"
    printfn "2. Paper"
    printfn "3. Scissors"
    printfn "4. Lizard"
    printfn "5. Spock"
    
    let playerInput = Console.ReadLine()
    getWeaponFromNumber (int playerInput)

let playerPicksGUI value =
    $"Player picks: {value}."

let aiPicksGUI value =
    $"Computer picks: {value}."

let gameScoreGUI (scoreboard) =
    printfn $"Player score: %d{scoreboard.Player} and Computer score: %d{scoreboard.Computer} "
    


let gameEndedGUI (scoreboard, maxScore) =
    match scoreboard with
        | { Player = p; Computer = c } when p >= maxScore -> printfn "Player wins!"
        | { Player = p; Computer = c } when c >= maxScore -> printfn "Computer wins!"
        | _ -> printfn "Game tied!"
        
let roundEndedGUI outCome = 
    match outCome with
    | Player -> printfn "Player Won this round"
    | Computer -> printfn "Computer won this round"
    | Tie -> printfn "It was a tie"
    outCome

let scoreGUI score stats =
    printfn $" PLayer score {score.Player} and percentage {stats.PlayerPercentage}" 
    printfn $" Computer score {score.Computer} and percentage {stats.ComputerPercentage}" 
    printfn $" Total amount of games {score.Total} and percentages of ties {stats.TotalPercentage}"
    
