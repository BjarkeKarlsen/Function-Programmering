module Exam1.FetchStrings
open System.Net.Http

let downloadFileAsync (url: string) =
    async {
        use client = new HttpClient()
        try
            let! content = Async.AwaitTask(client.GetStringAsync(url))
            return content
        with
        | ex ->
            printf "Error %s" ex.Message
            return ex.Message
    } |> Async.RunSynchronously

