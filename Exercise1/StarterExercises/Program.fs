module starter_exercise


// Exercise 1.2
let h xy =
    let x, y = xy
    sqrt(x**2.0 + y**2.0)
    
// Exercise 1.5
let rec fib = function
    | 0 -> 0
    | 1 -> 1
    | n -> fib(n-1) + fib(n-2)

// Exercise 1.6
let rec sum = function
    | (m, 0) -> m
    | (m, n) -> m+n + sum(m, n-1)

// Exercise 2.2
let rec pow (s, n) =
    match n with
    | n when n <= 0 -> ""
    | 1 -> s
    | n -> s + pow (s, (n-1))

// Version 2
let powWithBuilder (s : string) n =
    let builder = System.Text.StringBuilder()
    for count = 1 to n do
        builder.Append(s) |> ignore
    builder.ToString()

// Exercise 2.3
let isIthChar (str:string) i ch =
    match i with
    | index when index < 0 || index >= str.Length -> failwith ("Out of range")
    | _ ->(str[i] = ch)
    
// Exercise 2.5
let occFromIth (str: string, i, ch) =
    match i with
    | index when index < 0 || index >= str.Length -> 0
    | index -> str.Substring index 
            |> Seq.filter(fun charAtIndex -> charAtIndex = ch)
            |> Seq.length


// Exercise 2.8

let c = (1.0, 2.0)

[<EntryPoint>]
let main argv =

    printfn $"Exercise 1.2: %f{h (1.9, 5.7)}"
    
    printfn $"Exercise 1.5: %d{fib 0}"
    printfn $"Exercise 1.5: %d{fib 1}"
    printfn $"Exercise 1.5: %d{fib 5}"
    
    printfn $"Exercise 1.6: %d{sum (8, 0)}"
    printfn $"Exercise 1.6: %d{sum (8, 4)}"
    
    printfn $"Exercise 2.2: %d{pow ('h', 2)}"
    printfn $"Exercise 2.2: %s" powWithBuilder ('h', 2)
    
    printfn $"Exercise 2.2: %s" isIthChar ("hhsed", 2, )
    printfn $"Exercise 2.2: %s" powWithBuilder ('h', 2)
    
    0