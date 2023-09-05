module StarterExercises.Program
open Library

[<EntryPoint>]
let main argv =
    printfn($"%b{notDivisible (2, 5)}")
    printfn($"%b{notDivisible (3, 9)}")
    printfn($"%b{notDivisible (9, 3)}")
    0