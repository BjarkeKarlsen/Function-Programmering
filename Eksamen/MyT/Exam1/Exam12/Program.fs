// For more information see https://aka.ms/fsharp-console-apps
open System
open Microsoft.FSharp.Reflection
type Weapon =
    Rock | Paper | Scissors | Lizard | Spock

type Outcome =
    | Win
    | Lose
    | Tie

type Score = { Player: int; Computer: int}




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
let generateComputerChoice =
    unpackWeapon |>
    (fun weapons -> weapons.[Random().Next(weapons.Length)])
    
let generatePlayerChoice index =
    let weapons = unpackWeapon
    match List.tryItem index weapons with
    | Some weapon -> weapon
    | None -> failwithf "Invalid input"

let printWeaponOptions =
    printfn "Choose a weapon:"
    printfn "0. Rock"
    printfn "1. Paper"
    printfn "2. Scissors"
    printfn "3. Lizard"
    printfn "4. Spock"
    
    
let readPlayerInput () =
    Console.ReadLine() |> int
    

    
// let getPlayerChoice =
//     let playerInput 

let playerPick choice =
    printf $" Player picked: {choice} "
    choice 
    

let computerPick choice =
    printf $" Computer picked: {choice} "
    choice
    
 
let playerChoice =
    printWeaponOptions
    |> readPlayerInput
    |> generatePlayerChoice
    |> playerPick
    
    
let computerChoice =
    generateComputerChoice |> computerPick
    
    
let updateScore currentScore outcome  =
    match outcome with
    | Win ->  {currentScore with Player = currentScore.Player + 1 }
    | Lose -> { currentScore with Computer = currentScore.Computer + 1 }
    | Tie -> currentScore

let printScore score =
    printfn "Player: %d, Computer: %d" score.Player score.Computer
    
// Scoreboard

let gameLoop =
    //

    

[<EntryPoint>]

    let main _ =
        
        // let playerinput = playerChoose
        // playerPick playerinput
        let playerinput = playerChoice
        
        let computerinput = computerChoice
        
        let scoreoutcome = outcome playerinput computerinput
        let initialScore = { Player = 0; Computer = 0 }
        let updatedScore = updateScore initialScore scoreoutcome
        printScore updatedScore


        0