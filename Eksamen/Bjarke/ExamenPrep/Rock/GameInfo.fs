module ConsoleApp1.GameInfo

type Score =  { Player: int; Computer: int; Total: int }
type State =  { PlayerPercentage : float; ComputerPercentage: float; TotalPercentage: float }


type OutCome =
    | Player
    | Computer
    | Tie

let updateScore point score =
    match point with
        | Player -> { score with Player = score.Player + 1; Computer = score.Computer; Total = score.Total + 1}
        | Computer -> { score with Player = score.Player; Computer = score.Computer + 1; Total = score.Total + 1}
        | Tie -> { score with Player = score.Player; Computer = score.Computer; Total = score.Total + 1}
    
let calculatePercentage score =
    let totalGames = float score.Total
    let playerWins = float score.Player / totalGames
    let computerWins = float score.Computer / totalGames
    let ties = float (score.Total - score.Player - score.Computer) / totalGames
    { PlayerPercentage = playerWins; ComputerPercentage = computerWins; TotalPercentage =ties}
    

