module ConsoleApp1.Gameloop 

open GameInfo
open Weapon
open ConsoleGUI

let outCome (weapon1, weapon2) =
    match weapon1, weapon2 with
    | Rock, Scissors | Rock, Lizard -> Player
    | Scissors, Paper | Scissors, Lizard -> Player
    | Paper, Rock | Paper, Spock -> Player
    | Lizard, Spock | Lizard, Paper -> Player
    | Spock, Rock | Spock, Scissors -> Player
    | weapon1, weapon2 when weapon1 = weapon2 -> Tie
    | _ -> Computer

let run =     
    let mutable scoreboard = { Player = 0; Computer = 0; Total = 0 }
    let maxScore = 10
    
    
    // Flow
    let player = playerChoiceGUI
    let computer = generateChoice
    printfn $"%s{playerPicksGUI player}"
    printfn $"%s{aiPicksGUI computer}"
    
    let scoreboard = outCome(player, computer)
                    |> roundEndedGUI
                    |> (fun newScore -> updateScore newScore scoreboard)
    
    scoreGUI scoreboard (calculatePercentage scoreboard)
    
    
    
    0

