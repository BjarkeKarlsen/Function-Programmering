module Problem1.Http

open System.Net.Http

let httpClient = new HttpClient()

let GetSudoku =
    async {
        let! response = httpClient.GetAsync("https://sudoku-api.vercel.app/api/dosuku") |> Async.AwaitTask
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return string content
    }
    