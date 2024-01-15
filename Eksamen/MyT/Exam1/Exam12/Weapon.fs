module Exam12.Weapon
//
// open System
// open Microsoft.FSharp.Reflection
//
// type Weapon =
//     Rock | Paper | Scissors | Lizard | Spock
//     
// let getCases<'T> () =
//     typeof<'T>
//     |> FSharpType.GetUnionCases
//     |> Array.map( fun caseInfo -> FSharpValue.MakeUnion(caseInfo, [||]))
//
//
// let unpackWeapon =
//        let weapons : Weapon list = getCases<Weapon>() |> Array.map unbox |> Array.toList
//        weapons
//        
// let generateComputerChoice (random: Random) =
//     unpackWeapon |>
//     (fun weapons -> weapons.[random.Next(weapons.Length)])
//     
// let generatePlayerChoice index =
//     let weapons = unpackWeapon
//     match List.tryItem index weapons with
//     | Some weapon -> weapon
//     | None -> failwithf "Invalid input"