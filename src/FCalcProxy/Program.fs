module ProxyCalculator

open System
open System.Text
open System.Text.Json
open System.IO
open System.Net.Http
open Newtonsoft.Json

type AsyncMaybeBuilder() =
    // Async<option<a>> -> (a -> Async<option<b>>) -> Async<option<b>>
    member this.Bind(x, f) =
        async {
            let! maybe = x
            match maybe with
            | Some value -> return! f value
            | None -> return None
        }
        
    // a -> Async<option<a>>
    member this.Return(x) =
        async {
            return Some x
        }
        
let asyncMaybe = AsyncMaybeBuilder()

let createRequest a op b = 
    sprintf "http://localhost:5000?a=%i&b=%i&op=%A" a b op

type Op = 
    | Add
    | Sub
    | Mul
    | Div

type Res = {
    isOk: bool
    result: int
}

let toMaybe res =
    if res.isOk then
        Some res.result
    else None

// str -> Async<Maybe>
let getResponse a op b =
    async {
        use client = new HttpClient()
        let url = createRequest a op b
        let! response = client.GetAsync(url) |> Async.AwaitTask
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        let res = JsonConvert.DeserializeObject<Res> content
//        printfn "A %A" res
        return toMaybe res
    }

let printMaybe opt =
    match opt with
    | Some a -> printfn "Answer: %i" a
    | None -> printfn "Division by 0"

[<EntryPoint>]
let main (argv: string []): int =
    asyncMaybe {
        let! a' = getResponse 1 Add 2 
        let! a' = getResponse a' Add 3
        let! a' = getResponse a' Mul 10
        let! a' = getResponse a' Div 0
        return a'
    } |> Async.RunSynchronously |> printMaybe
    0
