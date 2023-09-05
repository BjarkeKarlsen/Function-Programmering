namespace StarterExercises
open System

module Library =
        //2.6      
        // If d < n then d will never be divisible by n
        let notDivisible (d, n) =
            if (abs(d) < abs(n)) then
                true
            else
                not (d % n = 0)
        
        // 2.7.1
        let rec test (a,b,c) =
            match (a,b) with
            | _ when a > b -> failwith(String.Format($"a should <= b: {a} <= {b}"))
            | _ when a < b -> notDivisible(a, c) && test(a+1, b, c)
            | _ -> notDivisible(a, c)
            
        // 2.7.2
        // Mistake in book. Should be c,a not a,c like in 2.7.1
        let rec test_prime(a, b, c) =
            match (a, b) with
            | _ when a > b -> failwith(String.Format($"a should <= b: {a} <= {b}"))
            | _ when a < b -> notDivisible(c, a) && test_prime(a+1, b, c)
            | _ -> notDivisible(c,a)
            
            
        let prime n =
            match n with
            | 1 -> true
            | 2 -> true
            | 3 -> true
            | _ when n < 1 -> false
            | _ -> test_prime(2, (float)n |> sqrt |> Math.Floor |> (int), n)
        
        
        // 2.7.3
        let rec nextPrime n =
            let isPrime = prime(n + 1)
            if isPrime then
                n + 1
            else
                nextPrime(n + 1)
                
        // 2.8 Binomial coefficients
        let rec bin(n, k) =
            match(n ,k) with
            | (row, 0) -> 1
            | (row, col) when col = n -> 1
            | (row, col ) -> bin(n - 1, k - 1) + bin(n - 1, k)

        // 2.9
        let rec f = function
            | (0, y) -> y
            | (x, y) -> f(x - 1, x * y)
            
            // 1. The inferred type is int*int -> int
            // 2. The function terminates for values (x, y) where x >= 0
            // 3.   // f(2, 3)
                    // ~> f(2-1, 3*2)
                    // ~> f(1-1, 1*6)
                    // ~> f(0, 6)
            // 4.         
        
        // 2.10             