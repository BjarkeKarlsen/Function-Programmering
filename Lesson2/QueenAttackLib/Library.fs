namespace QueenAttackLib

module Library =
    
    let create (n, m) =
        let size = 8
        let checkCoordinate = function
            | x when x >= 0 && x < size -> true
            | _ -> false
        checkCoordinate n && checkCoordinate m
        
    let canAttack (x1,y1) (x2,y2) =
        // Define the direction
        let row =  x1 = x2
        let column = y1 = y2
        let southWest (x,y) = (x-1, y+1)
        let northEast (x,y) = (x+1, y-1)
        let northWest (x,y) = (x-1, y-1)
        let southEast (x,y) = (x+1, y+1)
        // Recursive call the direction. If the queen is not on the map then it is false
        // If the queen hit the other queen then true. And else call the methode again
        let rec Diagonal direction = function   
            | (x,y) when not (create(x,y)) -> false
            | (x,y) when y2 = y && x2 = x -> true
            | (x,y) -> Diagonal direction (direction(x,y))
        
        // This is the output, only one of the have to be false
        row || column
        || Diagonal southWest (x1,y1)
        || Diagonal northEast (x1,y1)
        || Diagonal northWest (x1,y1)
        || Diagonal southEast (x1,y1)

