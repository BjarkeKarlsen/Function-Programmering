module Exam12.Prog
// For more information see https://aka.ms/fsharp-console-apps
open System
open Microsoft.FSharp.Reflection
type Weapon =
    Rock | Paper | Scissors | Lizard | Spock

type Outcome =
    | Win
    | Lose
    | Tie


type Score = { Player: int; Computer: int; Rounds: int}

type Percentages =  { PlayerPercentage : float; ComputerPercentage: float}



let getCases<'T> () =
    typeof<'T>
    |> FSharpType.GetUnionCases
    |> Array.map( fun caseInfo -> FSharpValue.MakeUnion(caseInfo, [||]))


let outcome playerweapon computerweapon =
    match playerweapon, computerweapon with
    | Rock, Paper | Paper, Scissors | Scissors, Rock
    | Rock, Spock | Paper, Lizard | Scissors, Spock
    | Lizard, Rock | Lizard, Scissors | Spock, Paper ->
        Lose
    | Rock, Rock | Paper, Paper | Scissors, Scissors | Lizard, Lizard | Spock, Spock ->
        Tie
    | _ ->
        Win
    
  

let unpackWeapon =
       let weapons : Weapon list = getCases<Weapon>() |> Array.map unbox |> Array.toList
       weapons
let generateComputerChoice (random: Random) =
    unpackWeapon |>
    (fun weapons -> weapons.[random.Next(weapons.Length)])
    
let generatePlayerChoice index =
    let weapons = unpackWeapon
    match List.tryItem index weapons with
    | Some weapon -> weapon
    | None -> failwithf "Invalid input"

let printOptions =
    printfn "Choose a weapon:"
    printfn "0. Rock"
    printfn "1. Paper"
    printfn "2. Scissors"
    printfn "3. Lizard"
    printfn "4. Spock"
    printfn "Press q quit"
    


    
// let getPlayerChoice =
//     let playerInput 

let playerPick choice =
    printf $" Player picked: {choice} "
    choice 
    

let computerPick choice =
    printf $" Computer picked: {choice} "
    choice
    
 
let playerChoice =
    printOptions
    |> (fun () -> Console.ReadLine() |> int)
    |> generatePlayerChoice
    |> playerPick
    
    
// let computerChoice =
//     generateComputerChoice |> computerPick
    
    
let updateScore currentScore outcome  = 
    let updatedScore =
        match outcome with
        | Win ->  {currentScore with Player = currentScore.Player + 1 }
        | Lose -> { currentScore with Computer = currentScore.Computer + 1 }
        | Tie -> { currentScore with Player = currentScore.Player + 1; Computer = currentScore.Computer + 1}
    { updatedScore with Rounds = updatedScore.Rounds + 1 }

    

let printScore score =
    printfn $"Player: %d{score.Player}, Computer: %d{score.Computer}. Rounds: %d{score.Rounds}"
    
// Scoreboard

// let endGame =
//     printfn $"Totals rounds: {Rounds}"



// count total rounds
//if total rounds = 5 or esc, end game
// type should be either round or input
//print scores and winner


let round (initialScore: Score, random: Random ) =
    let playerinput =
        printOptions
        |> (fun () -> Console.ReadLine() |> int)
        |> generatePlayerChoice
        |> playerPick
    let computerinput = generateComputerChoice random |> computerPick
    let scoreoutcome = outcome playerinput computerinput
    let updatedScore = updateScore initialScore scoreoutcome
    printScore updatedScore
    updatedScore
    

let calculatePercentages score totalPossible =
    let totalGames = float totalPossible
    let playerPercentage = (float score.Player / totalGames) * 100.0
    let computerPercentage = (float score.Computer / totalGames) * 100.0
    { PlayerPercentage = playerPercentage; ComputerPercentage = computerPercentage }

let scoreGUI score stats =
    printfn $" Player score {score.Player} and percentage {stats.PlayerPercentage}%%" 
    printfn $" Computer score {score.Computer} and percentage {stats.ComputerPercentage}%%" 
let scoreBoard score =
    printfn "Game Over! "
    let totalPossible = 5 // Set the total possible games here
    calculatePercentages score totalPossible
    |> scoreGUI score
    |> fun _ ->
        match score.Player with
        | p when p >= 3 -> printfn "Player Wins!"
        | _ -> printfn "Computer Wins!"
    

    
let rec gameLoop (score: Score, random: Random) =
        match score.Rounds with
        | 5 -> scoreBoard score 
        | _ ->
            let userInput = Console.ReadLine()
            match userInput with
            | "q" -> scoreBoard score
            | _ ->
            let updatedScore = round (score,random) 
            gameLoop (updatedScore, random)
            
let runGame =
    let initialScore = { Player = 0; Computer = 0; Rounds = 0}
    let random = Random()
    gameLoop (initialScore, random)
    

[<EntryPoint>]

let main _ =
        
        runGame
        // // let playerinput = playerChoose
        // // playerPick playerinput
        // let playerinput = playerChoice
        //
        // let computerinput = computerChoice
        //
        // let scoreoutcome = outcome playerinput computerinput
        // // let initialScore = { Player = 0; Computer = 0; Rounds = 0}
        // let updatedScore = updateScore initialScore scoreoutcome
        // printScore updatedScore
        0