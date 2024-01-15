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
    
type AIAction =
    | RandomMove
    | CounterMove
    | EqualMove
    
type AIState = {
    playerMoves: Dictionary<Weapon, int> 
    aiAction: AIAction
    round: int
}

// let initAIState () =
//     let weapons = getWeapons
//     let initialPlayerMoves = weapons |> List.map (fun w -> w, 0) |> dict
//     { playerMoves = initialPlayerMoves
//       aiAction = RandomMove
//       round = 0  }


let updateAI weapon aiState =
    // Should be done something here else
    let newAction = AIAction.CounterMove
    let mutable mutablePlayerMoves = Dictionary<Weapon, int>(aiState.playerMoves)

    if mutablePlayerMoves.ContainsKey(weapon) then
        mutablePlayerMoves.[weapon] <- mutablePlayerMoves.[weapon] + 1

    let newAiState = {
        aiState with
            aiAction = newAction
            round = aiState.round + 1
            playerMoves = mutablePlayerMoves
    }

    newAiState
    

let findKeysWithMaxValue (dict: IDictionary<'a, 'b>) =
    let maxValue = 
        dict 
        |> Seq.cast<KeyValuePair<'a, 'b>> 
        |> Seq.map (fun kvp -> kvp.Value) 
        |> Seq.max

    dict
    |> Seq.cast<KeyValuePair<'a, 'b>>
    |> Seq.filter (fun kvp -> kvp.Value = maxValue)
    |> Seq.map (fun kvp -> kvp.Key)

let getCounterMove dict =
    dict
    |> Seq.cast<KeyValuePair<Weapon, int>> // Ensure correct type
    |> Seq.maxBy (fun kvp -> kvp.Value)
    |> (fun kvp -> kvp.Key)
    // dict 
    //     |> Seq.cast<KeyValuePair<'a, 'b>> 
    //     |> Seq.map (fun kvp -> kvp.Value) 
    //     |> Seq.max

let getCounterMoves dict aiState =
    let values = findKeysWithMaxValue dict
    
    match values with
    | values when Seq.length values > 1 -> getCounterMove values
    | values -> values |> Seq.head
         
        
let makeAIMove aiState random =
    let counterMoves = getCounterMoves aiState.playerMoves
    
    match aiState.aiAction with
    | RandomMove -> generateChoice random
    | CounterMove -> getCounterMove aiState.playerMoves
    

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