module ConsoleApp1.Weapon
    
open Microsoft.FSharp.Reflection
open System
open System.Collections.Generic


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

let getWeapons =
    let weapons : Weapon list = getCases<Weapon>()
                                |> Array.map unbox
                                |> Array.toList
    weapons

let generateChoice (random: Random) =
    let weapons = getWeapons
    weapons.[random.Next(weapons.Length)]
    
let getWeaponFromNumber index =
    let weapons : Weapon list = getWeapons
    match List.tryItem index weapons with
    | Some weapon -> weapon
    | None -> failwithf "Invalid input"
    
    
type AIState = {
    playerMoves: IDictionary<Weapon, int>
    aiAction: IDictionary<Action, int>
}

type AIAction =
    | RandomMove
    | CounterMove


let initAIState () =
    let weapons = getWeapons
    let initialPlayerMoves = weapons |> List.map (fun w -> w, 0) |> dict
    let initialState = 
    { playerMoves = initialPlayerMoves
      round = failwith "todo" }

let updatePlayerMoves weapon aiState =
    aiState.playerMoves.[weapon] <- aiState.playerMoves.[weapon] + 1)
    
    // Which state dependt on int
    aiState.aiAction <- aiState.aiAction + 1
    
    aiState

let findKeyWithMaxValue (dict: IDictionary<'a, 'b>) =
    dict
    |> Seq.cast<KeyValuePair<'a, 'b>>
    |> Seq.maxBy (fun kvp -> kvp.Value)
    |> (fun kvp -> kvp.Key)


let getCounterMoves dict aiState =
    findKeyWithMaxValue dict

let makeAIMove aiState dict random =
    let counterMoves = getCounterMoves dict
    
    match aiState with
    | RandomMove -> generateChoice random
    | CounterMove -> counterMoves
    

// let updateAIState weapon aiState =
//     let updatedPlayerMoves =
//         aiState.playerMoves
//         |> Map.add weapon (Map.findDefault 0 weapon aiState.playerMoves + 1)
//     let updatedCounterMoves =
//         aiState.counterMoves
//         |> getCounterMoves [weapon] aiState.playerMoves
//         |> List.fold (fun acc (counterMove, _) ->
//             Map.add counterMove (Map.findDefault 0 counterMove acc + 1) acc
//         ) aiState.counterMoves
//     { playerMoves = updatedPlayerMoves; counterMoves = updatedCounterMoves }





// let recordCounterMove weapon counterMove aiState =
//     let counterMoveEntry =
//         match counterMove with
//         | Rock _ -> aiState.counterMoves.TryGetValue(Rock , 0)
//         | Paper _ -> aiState.counterMoves.TryGetValue(Paper , 0)
//         | Scissors _ -> aiState.counterMoves.TryGetValue(Scissors , 0)
//         | Lizard _ -> aiState.counterMoves.TryGetValue(Lizard , 0)
//         | Spock _ -> aiState.counterMoves.TryGetValue(Spock , 0)
//
//     aiState.counterMoves.[counterMove] <- counterMoveEntry.Value + 1




// let makeAIMove topPicks aiState random =
//     let counterMoves = getCounterMoves topPicks aiState
//     let randomCounterMove = counterMoves |> Seq.toArray |> Array.get (random.Next(counterMoves |> Seq.length))
//     match randomCounterMove with
//     | Rock weapon -> weapon
//     | Paper weapon -> weapon
//     | Scissors weapon -> weapon
//     | Lizard weapon -> weapon
//     | Spock weapon -> weapon