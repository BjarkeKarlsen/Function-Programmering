module Problem1.Game
open System
open Problem1.Type
open Problem1.Content1


let print (board:Board) =
    for row in 0..8 do
        for column in 0..8 do
            match List.item row (List.item column board) with
            | Some n-> printf $"{n}"
            | None -> printf "."
            if (column + 1) % 3 = 0 && column <> 8 then printf "| "
        printfn ""    
        if (row + 1) % 3 = 0 && row <>8 then printfn "------+-------+------"
        


let update (x: int) (y: int) (cellValue: Cell) (board: Board) =
      match x, y with
        | x, y when x < 0 || x > 8 || y < 0 || y > 8 -> board
        | _ ->
            board
            |> List.mapi (fun rowIndex row ->
                match rowIndex with
                | y' when y' = y ->
                    row
                    |> List.mapi (fun columnIndex cell ->
                        match columnIndex with
                        | x' when x' = x -> cellValue
                        | _ -> cell
                    )
                | _ -> row
            )


let isValidInput (x: int) (y: int) (value: int) : bool =
    match x, y, value with
    | x, _, _ when x < 0 || x > 8 -> false
    | _, y, _ when y < 0 || y > 8 -> false
    | _, _, value' when value' < 1 || value' > 9 -> false
    | _ -> true


let rec gameloop (board: Board) : Board =
        // Print the current state of the board
        print board

        // Ask the user for input
        printfn "Press S to start sudoku:"
        printfn "Press Q to quit"
        let input = Console.ReadLine().ToLower()
        
        match input with
            |"s" ->
                printfn "Enter a coordinate x and y from the range 0 - 8 and a value from the range 1-9."
                printfn "Console example: 1 3 9"
                let coordinate = Console.ReadLine()
                match coordinate with
                    | coordinate when coordinate.Length <> 3 ->
                        printfn "Invalid input"
                        gameloop board
                    | _ ->
                        let x = int coordinate.[0]
                        let y = int coordinate.[1]
                        let value = int coordinate.[2]
                        if x >= 0 && x <= 8 && y >= 0 && y <= 8 && value >= 1 && value <= 9 then
                           let updateBoard = update x y (Some value) board
                           match check updateBoard with
                            | Ok _ ->  gameloop updateBoard
                            | Error errors ->
                                // If the board is invalid, print the errors and continue the loop with the original board
                                printfn "The following errors occurred:"
                                errors |> List.iter (printfn "%s")
                                gameloop updateBoard
                        else
                            printfn "invalid input"
                            gameloop board
       

        
  
