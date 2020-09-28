module Library

open System

type Op =
| Add
| Sub
| Mul
| Div

let calc a b op = 
    match op with
    | Add -> Ok (a + b)
    | Sub -> Ok (a - b)
    | Mul -> Ok (a * b)
    | _  -> if b <> 0 then Ok (a / b) else Error DivideByZeroException

let print res =
    match res with
    | Ok value -> printf "Ok, value: %i" value
    | Error _ -> printf "Error, div by zero"
