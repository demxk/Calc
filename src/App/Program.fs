open System.Numerics
open System.Drawing
open System
open FSharp.Data
open FSharp.Data.JsonExtensions
open FsHttp
open Library
open Microsoft.FSharp.Math
open System.Windows.Form

let mutable mx = -1.5
let mutable my = -1.5

[<EntryPoint>]
let main argv =
    maybe {
        let x = 1
        let! y = x |> Add 2
        let! z = y |> Mul 3
        let! z = z |> Sub 6
        return! (z |> Div 1)
    } |> printfn "answer is %A"
    maybe {
        let x = 4
        let! y = x |> Sub 2
        let! z = y |> Div 2
        let! z = z |> Add 3
        return! (z |> Div 0)
    } |> printfn "answer is %A"
    
    Application.Run(createImage(1.5, mx, my, 20))

let cMax = complex 1.0 1.0
let cMin = complex -1.0 -1.0

let scalingFactor s = s * 1.0 / 200.0

let mapPlane (x, y, s, mx, my) =
    let fx = ((float x) * scalingFactor s) + mx
    let fy = ((float y) * scalingFactor s) + my
    complex fx fy

let colorize c =
    let r = (4 * c) % 255
    let g = (6 * c) % 255
    let b = (8 * c) % 255
    Color.FromArgb(r,g,b)

let rec isInMandelbrotSet (z, c, iter, count) =
    if (cMin < z) && (z < cMax) && (count < iter) then
        isInMandelbrotSet ( ((z * z) + c), c, iter, (count + 1) )
    else count
let createImage (s, mx, my, iter) =
    let image = new Bitmap(400, 400)
    for x = 0 to image.Width - 1 do
        for y = 0 to image.Height - 1 do
            let count = isInMandelbrotSet( Complex.Zero, (mapPlane (x, y, s, mx, my)), iter, 0)
            if count = iter then
                image.SetPixel(x,y, Color.Black)
            else
                image.SetPixel(x,y, colorize( count ) )
    let temp = new Form() in
    temp.Paint.Add(fun e -> e.Graphics.DrawImage(image, 0, 0))
    temp
