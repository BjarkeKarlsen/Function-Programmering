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

type Action =
    | PlayRound
    | MaxRounds
    | Quit

type Score = { Player: int; Computer: int; Rounds: int}

type Percentages =  { PlayerPercentage : float; ComputerPercentage: float; TiePercentage: float}



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
        | Tie -> currentScore
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
        Some ((generatePlayerChoice (tryToInt player |> Option.get)))
    | _ ->
        printfn "Enter a valid number between 1 and 5"
        action ()
        
        
        
let updateAction total =
    let maxRounds = 10

    match total with
    | value when value >= maxRounds -> MaxRounds
    | _ -> PlayRound
    

let playRound initialScore random =
    let playerAction = action()
    
    match playerAction with
    | None -> Quit, initialScore
    | Some weapon ->
        playerPick weapon
        let computerinput = generateComputerChoice random |> computerPick
        let scoreoutcome = outcome weapon computerinput
        let updatedScore = updateScore initialScore scoreoutcome
        printScore updatedScore
        let newAction = updateAction updatedScore.Rounds
        newAction, updatedScore
        

// let round (initialScore: Score, random: Random ) =
//     let playerinput =
//         printOptions
//         |> (fun () -> Console.ReadLine() |> int)
//         |> generatePlayerChoice
//         |> playerPick
//     let computerinput = generateComputerChoice random |> computerPick
//     let scoreoutcome = outcome playerinput computerinput
//     let updatedScore = updateScore initialScore scoreoutcome
//     printScore updatedScore
//     updatedScore
//     

let calculatePercentages score =
    let totalGames = float score.Rounds
    let playerPercentage = (float score.Player / totalGames) * 100.0
    let computerPercentage = (float score.Computer / totalGames) * 100.0
    let tiePercentage = ((float score.Rounds - float score.Player - float score.Computer)/ totalGames) * 100.0
    { PlayerPercentage = playerPercentage; ComputerPercentage = computerPercentage; TiePercentage = tiePercentage }

let scoreGUI score stats =
    printfn $" Player score {score.Player} and percentage {stats.PlayerPercentage}%%" 
    printfn $" Computer score {score.Computer} and percentage {stats.ComputerPercentage}%%"
    printfn $" Rounds: {score.Rounds} Tie percentage {stats.TiePercentage}%%"

let scoreBoard score =
    printfn "Game Over! "
    calculatePercentages score
    |> scoreGUI score
    |> fun _ ->
        match score.Player with
        | p when p >= 3 -> printfn "Player Wins!"
        | _ -> printfn "Computer Wins!"
    

    
let rec gameLoop action scoreboard random =
        match action with
        | PlayRound -> 
            playRound scoreboard random
            |> fun ( newAction, newScoreboard) -> gameLoop newAction newScoreboard random
        | MaxRounds | Quit -> 
            scoreGUI scoreboard (calculatePercentages scoreboard)
            |> (fun _ -> 0)
            
let runGame =
    let action = Action.PlayRound
    
    let initialScore = { Player = 0; Computer = 0; Rounds = 1}
    let random = Random()
    gameLoop action initialScore random



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