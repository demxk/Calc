module Library

open System

type Error = 
    | DivideByZero
    | ParseError

type Either<'T> =
    | Ok of 'T
    | Err of Error

type ResultBuilder() =

    member this.Bind(expr, f) =
        match expr with
        | Ok v -> f v
        | Err e -> Err e

    member this.Return(value) = Ok value

    member this.ReturnFrom(m) = m

let result = ResultBuilder()

let Add x y = Ok (y + x)
let Sub x y = Ok (y - x)
let Mul x y = Ok (y * x)
let Div x y = 
    match x with
    | 0 -> Err DivideByZero
    | _ -> Ok (y / x)


let printCal m =
    match m with
    | Ok value -> printfn "Ok -> %i" value
    | Err err -> match err with
                 | DivideByZero -> printfn "Error! -> Division by 0"
                 | _ -> printfn "Error! -> Parse error"

