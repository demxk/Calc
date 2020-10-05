// Learn more about F# at http://fsharp.org

open System.Numerics
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
    let mandelbrot (n: int) (c: Complex): seq<'a> =
        Complex(0., 0.)
        |> Seq.unfold(fun z -> let newz = z * z + c; in if Complex.Abs(newz) < 4. then Some(newz, newz) else None)
        |> Seq.truncate(n)
    mandelbrot 5 (Complex(0.3, 0.6)) |> printf "%A"
    0