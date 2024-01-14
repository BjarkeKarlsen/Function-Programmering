module ExamenPrep.FetchStrings

open System.Net.Http

let getUrlAsync (url: string ) =
    async {
        use client = new HttpClient()
        
        try
            let! response = Async.AwaitTask(client.GetStringAsync(url))
            return response
        with
        | ex -> 
            // Handle exceptions if needed
            printfn $"Error: %s{ex.Message}"
            return ex.Message // 
    } |> Async.RunSynchronously
