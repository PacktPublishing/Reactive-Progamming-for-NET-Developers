namespace Cap09

module _01_Discrete_Components =
    
    open System.Windows.Forms
    
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.filter ( fun evArgs -> 
            evArgs.X > 100 && evArgs.Y > 100 && 
            evArgs.X < 255 && evArgs.Y < 255) 
        |> Event.add ( fun evArgs ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y))

    #r @"../packages/FSharp.Charting.0.90.14/lib/net40/FSharp.Charting.dll"
    #load @"..\packages\FSharp.Charting.0.90.14\FSharp.Charting.fsx"
    
    open System
    open System.Drawing
    open System.Windows.Forms
    open FSharp.Charting
    
    let list = [for x in -10 .. 10 -> (x, x * x)]
    Chart.Point(list,Name="Integer")



