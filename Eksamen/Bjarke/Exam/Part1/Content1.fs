module Part1.Content1

open Part1.Type
open System

let maxValueInCell = 45

let isNumberOfElement element =
    element < 0 || element > 8

let checkRow row board =
    match row with
    | row when isNumberOfElement row -> None
    | row ->
        let rowBox = List.item row board
        let rowValues = rowBox |> List.choose id
        let sum = rowValues |> List.sum
        let uniqueValues = rowValues |> Set.ofList |> Set.count
        match sum, uniqueValues with
        | sum, uniqueValues when sum <= maxValueInCell && uniqueValues = List.length rowValues -> Some true
        | _ -> None
        
let checkColumn column board =
    match column with
    | column when isNumberOfElement column -> None
    | column ->
        let columnBox = List.item column board
        let columnValues = columnBox |> List.choose id
        let sum = columnValues |> List.sum
        let uniqueValues = columnValues |> Set.ofList |> Set.count
        match sum, uniqueValues with
        | sum, uniqueValues when sum <= maxValueInCell && uniqueValues = List.length columnValues -> Some true
        | _ -> None
        
let checkBox box board =
    match box with
    | box when isNumberOfElement box -> None
    | box ->
        let startCornerRow = (box / 3) * 3
        let startCornerColumn = (box % 3) * 3
        let boxCells = 
            [ for row in startCornerRow .. startCornerRow + 2 do
                for column in startCornerColumn .. startCornerColumn + 2 do
                  yield List.item row board |> List.item column ]
        let boxValues = boxCells |> List.choose id
        let sum = boxValues |> List.sum
        let uniqueValues = boxValues |> Set.ofList |> Set.count
        match sum, uniqueValues with
        | sum, uniqueValues when sum <= maxValueInCell && uniqueValues = List.length boxValues -> Some true
        | _ -> None
   

        
let check board =
    let mutable errors = []

    for row in 0..8 do
        match checkRow row board with
        | None
        | Some false -> errors <- ("Row " + row.ToString() + " is not filled correctly") :: errors
        | _ -> ()

    for column in 0..8 do
        match checkColumn column board with
        | None
        | Some false -> errors <- ("Column " + column.ToString() + " is not filled correctly") :: errors
        | _ -> ()

    for box in 0..8 do
        match checkBox box board with
        | None
        | Some false -> errors <- ("Box " + box.ToString() + " is not filled correctly") :: errors
        | _ -> ()

    match errors with
    | errors when List.isEmpty errors -> Ok board
    | errors -> Error errors

let makeVerticalWall value = 
    match value with
        | i when (i + 1) % 3 = 0 && i <> 8 -> printf "| " 

let makeHorizontalWall value =
    match value with
        | i when (i + 1) % 3 = 0 && i <> 8 -> printfn "------+-------+------" 

let print board =
    printfn "====================="
    for row in 0..8 do
        for column in 0..8 do
            match List.item column (List.item row board) with
            | None -> printf ". "
            | Some value -> printf $"%d{value}"
            makeVerticalWall column
        printfn ""
        makeHorizontalWall row
    printfn "====================="
    
let update x y cellValue board =
    match x, y with
    | x, y when isNumberOfElement x || isNumberOfElement y -> board
    | x, y ->
         board
        |> List.mapi (fun rowIndex row ->
            match rowIndex with
            | rowIndex when rowIndex = y -> // If this is the row to update, map over the cells in the row
                row
                |> List.mapi (fun columnIndex cell ->
                    match columnIndex with 
                    | columnIndex when columnIndex = x -> cellValue // If this is the cell to update, replace the cell value with the new value
                    | _ -> cell // // If this is not the cell to update, keep the original cell value
                )
            | _ -> row// If this is not the row to update, keep the original row
        )

let rec loop board =
    print board
    
    let input = Console.ReadLine().Split(' ')

    // Validate the user input
    match input with
    | input when input.Length <> 3 ->
        printfn "Invalid input. row (0-8), column (0-8), and value (1-9) [x y value]."
        loop board
    | _ ->
       
        let x = int input.[0]
        let y = int input.[1]
        let value = int input.[2]

        match x, y, value with
        | x, _, _ when isNumberOfElement x ->
            printfn "Invalid input. Row should be between 0 and 8 and was ."
            loop board
        | _, y, _ when isNumberOfElement y ->
            printfn "Invalid input. Column should be between 0 and 8"
            loop board
        | _, _, value when value < 1 || value > 9 ->
            printfn "Invalid input. Value should be between 1 and 9."
            loop board
        | _ -> 
            let updatedBoard = update x y (Some value) board

            match check updatedBoard with
            | Ok _ ->
                loop updatedBoard
            | Error errors ->
                printfn "The following errors occurred:"
                errors |> List.iter (printfn "%s")
                loop board  
    
let findEmptyCell (board: Board)   =
    board
    |> List.mapi (fun i row -> row |> List.mapi (fun j cell -> (i, j, cell)))
    |> List.concat
    |> List.tryFind (fun (_, _, cell) -> cell.IsNone)
        
let rec solveSudoku board =
    match findEmptyCell board with
    | None -> Some board
    | Some (row, column, _ ) ->
        [0..9]
        |> List.tryPick (fun number ->
            // Replace the empty cell with the current number
            let updatedBoard = update column row (Some number) board

            // Check if the updated board is valid
            match check updatedBoard with
            | Ok _ -> // If the board is valid, recursively solve the updated board
                match solveSudoku updatedBoard with
                | Some solvedBoard -> Some solvedBoard  // A valid solution is found
                | None -> None  // No solution was found, continue with the next number
            | Error _ -> None  // The board is not valid, continue with the next number
        )