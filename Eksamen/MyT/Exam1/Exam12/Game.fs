module Exam12.Game
// open System
// open Exam12.Weapon
// open Exam12.GameGUI
//
// type Outcome =
//     | Win
//     | Lose
//     | Tie
// let outcome playerweapon computerweapon =
//     match playerweapon, computerweapon with
//     | Rock, Paper | Paper, Scissors | Scissors, Rock
//     | Rock, Spock | Paper, Lizard | Scissors, Spock
//     | Lizard, Rock | Lizard, Scissors | Spock, Paper ->
//         Lose
//     | Rock, Rock | Paper, Paper | Scissors, Scissors | Lizard, Lizard | Spock, Spock ->
//         Tie
//     | _ ->
//         Win
//     
//   
// let playerChoice =
//     printWeaponOptions
//     |> (fun () -> Console.ReadLine() |> int)
//     |> generatePlayerChoice
//     |> playerPick
//     
// let updateScore currentScore outcome  = 
//     let updatedScore =
//         match outcome with
//         | Win ->  {currentScore with Player = currentScore.Player + 1 }
//         | Lose -> { currentScore with Computer = currentScore.Computer + 1 }
//         | Tie -> { currentScore with Player = currentScore.Player + 1; Computer = currentScore.Computer + 1}
//     { updatedScore with Rounds = updatedScore.Rounds + 1 }
//
// let round (initialScore: Score, random: Random ) =
//     let playerinput =
//         printWeaponOptions
//         |> (fun () -> Console.ReadLine() |> int)
//         |> generatePlayerChoice
//         |> playerPick
//     let computerinput = generateComputerChoice random |> computerPick
//     let scoreoutcome = outcome playerinput computerinput
//     let updatedScore = updateScore initialScore scoreoutcome
//     printScore updatedScore
//     updatedScore
//     
// let calculatePercentages score totalPossible =
//     let totalGames = float totalPossible
//     let playerPercentage = (float score.Player / totalGames) * 100.0
//     let computerPercentage = (float score.Computer / totalGames) * 100.0
//     { PlayerPercentage = playerPercentage; ComputerPercentage = computerPercentage }
//
// let scoreBoard score =
//     printfn "Game Over! Maximum rounds reached"
//     let totalPossible = 5 // Set the total possible games here
//     calculatePercentages score totalPossible
//     |> scoreGUI score
//     |> fun _ ->
//         match score.Player with
//         | p when p >= 3 -> printfn "Player Wins!"
//         | _ -> printfn "Computer Wins!"
//     
//
// let rec gameLoop (score: Score, random: Random) =
//         match score.Rounds with
//         | 5 -> scoreBoard score 
//         | _ ->
//             let updatedScore = round (score,random) 
//             gameLoop (updatedScore, random)
// let runGame =
//     let initialScore = { Player = 0; Computer = 0; Rounds = 0}
//     let random = Random()
//     gameLoop (initialScore, random)
//     
