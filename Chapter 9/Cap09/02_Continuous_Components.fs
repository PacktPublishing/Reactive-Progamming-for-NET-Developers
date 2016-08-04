namespace Cap09

module _02_Continuous_Components =
    
    #r @"../packages/FSharp.Charting.0.90.14/lib/net40/FSharp.Charting.dll"
    #load @"..\packages\FSharp.Charting.0.90.14\FSharp.Charting.fsx"
    
    open System
    open System.Drawing
    open System.Windows.Forms
    open FSharp.Charting
    
    let list = [for x in -10.0 .. 10.0 -> (x, x ** 2.0)]
    Chart.Line(list,Name="Float")


