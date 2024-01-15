module ConsoleApp1.ConsoleGUI

open System
open ConsoleApp1.GameInfo

let playerChoiceGUI =
    printfn "Choose a weapon:"
    printfn "0. Rock"
    printfn "1. Paper"
    printfn "2. Scissors"
    printfn "3. Lizard"
    printfn "4. Spock"
    printfn "q for quit"

let getPlayerInput =    
    Console.ReadLine()
    

let playerPicksGUI value =
    printfn $"Player picks: {value}."
    
let aiPicksGUI value =
    printfn $"Computer picks: {value}."
    value

let gameScoreGUI (scoreboard) =
    printfn $"Player score: %d{scoreboard.Player} and Computer score: %d{scoreboard.Computer} "

let gameEndedGUI (scoreboard, maxScore) =
    match scoreboard with
        | { Player = p; Computer = _ } when p >= maxScore -> printfn "Player wins!"
        | { Player = _; Computer = c } when c >= maxScore -> printfn "Computer wins!"
        | _ -> printfn "Game tied!"
        
let roundEndedGUI outCome = 
    match outCome with
    | Win -> printfn "Player Won this round"
    | Lose -> printfn "Computer won this round"
    | Tie -> printfn "It was a tie"
    outCome

let scoreGUI score stats =
    printfn $" PLayer score {score.Player} and percentage {stats.PlayerPercentage}" 
    printfn $" Computer score {score.Computer} and percentage {stats.ComputerPercentage}" 
    printfn $" Total amount of games {score.Total} and percentages of ties {stats.TotalPercentage}"
    
