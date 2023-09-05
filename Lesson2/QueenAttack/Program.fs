open System
open QueenAttackLib.Library

[<EntryPoint>]
let main argv =
    printf($"%b{canAttack (7,7) (4,7)}")
    0