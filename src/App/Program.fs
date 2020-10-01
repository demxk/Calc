// Learn more about F# at http://fsharp.org

open System
open System.IO
open Library

[<EntryPoint>]
let main argv =
    result {
        let x = 1
        let! y = x |> Add 2
        let! z = y |> Mul 3
        let! z = z |> Sub 6
        return! (z |> Div 1)
    } |> printCal
    result {
        let x = 4
        let! y = x |> Sub 2
        let! z = y |> Div 2
        let! z = z |> Add 3
        return! (z |> Div 0)
    } |> printCal
    
    0 // return an integer exit code
