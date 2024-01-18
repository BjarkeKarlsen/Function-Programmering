module Problem1.Content1
open Problem1.Type

let checkRow (row:int) (board: Board): bool option =
    match row with
     | row when row < 0 || row > List.length board -> None
     | _ ->
            let rowIndex = List.item row board
            let filteredRowValues = List.choose id rowIndex
            let sum = List.sum filteredRowValues
            let uniqueValuesCount = filteredRowValues |> Set.ofList |> Set.count
            if sum <= 45 && uniqueValuesCount = List.length filteredRowValues then Some true else None
          
          
         
    

let checkColumn (column: int) (board: Board): Option<bool> =
     match column with
     | column when column < 0 || column > List.length board -> None
     | _ ->
             let columnIndex = List.item column board
             let filteredColumnValues = List.choose id columnIndex
             let sum = List.sum filteredColumnValues
             let uniqueValuesCount = filteredColumnValues |> Set.ofList |> Set.count
             if sum <= 45 && uniqueValuesCount = List.length filteredColumnValues then Some true else None
    

let checkBox (box:int) (board:Board): Option<bool>=
     match box with
     | box when box < 0 || box > List.length board -> None
     | _ ->
             let rows, columns = (box / 3) * 3, (box % 3) * 3
             let box =
                    [ for row in rows .. rows + 2 do
                      for column in columns .. columns + 2 ->
                      List.item row board |> List.item column ]
             let values =  List.choose id box
             let sum = List.sum values
             if sum <= 45 && List.distinct values
                             |> List.length = List.length values then Some true else None
        
let check (board:Board): Result<board, string list> =
     
    let mutable errors = []
     
    let errorMsg cell =
         ($"{cell} is invalid: " + cell.ToString()) :: errors
     
    for column in 0..8 do
         match checkColumn column board with
         | None
         | Some false -> errors <- errorMsg column
         | _ -> printfn "Invalid"
         
    for row in 0..8 do
         match checkRow row board with
         | None
         | Some false -> errors <- errorMsg row
         | _ -> printfn "Invalid"
     
    for box in 0..8 do
        match checkBox box board with
        | None
        | Some false -> errors <- errorMsg box
        | _ -> printfn "Invalid"
                
    if List.isEmpty errors then Ok board else Error errors  
         