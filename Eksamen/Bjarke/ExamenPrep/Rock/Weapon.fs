module ConsoleApp1.Weapon
    
open Microsoft.FSharp.Reflection
open System

type Weapon =
    | Rock
    | Paper
    | Scissors
    | Lizard
    | Spock
    
let getCases<'T> () =
    typeof<'T>
    |> FSharpType.GetUnionCases
    |> Array.map (fun caseInfo -> FSharpValue.MakeUnion(caseInfo, [||]))

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