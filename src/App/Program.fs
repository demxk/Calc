// Learn more about F# at http://fsharp.org

open System
open Library

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    calc 1 0 Div |> print
    0 // return an integer exit code
