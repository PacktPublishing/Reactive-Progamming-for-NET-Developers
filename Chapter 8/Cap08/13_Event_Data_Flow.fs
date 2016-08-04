namespace Cap08

module _13_Event_Data_Flow =
    
    //Syntax type Event
    //type Event<'T> =
    //    class
    //     new Event : unit -> Event<'T>
    //     member this.Trigger : 'T -> unit
    //     member this.Publish :  IEvent<'T>
    //    end

    open System.Windows.Forms
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.filter ( fun evArgs -> 
            evArgs.X > 0 && evArgs.Y > 0 && 
            evArgs.X < 255 && evArgs.Y < 255) 
        |> Event.add ( fun evArgs -> 
            form.BackColor <- System.Drawing.Color.FromArgb(
                evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y))


