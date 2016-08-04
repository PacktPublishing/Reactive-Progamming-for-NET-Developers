namespace Cap08

module _09_Active_Patterns =
    
    //Syntax
    //let (|identifer1|identifier2|...|) [ arguments ] = expression
    //let (|identifier|_|) [ arguments ] = expression

    let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

    let (|BooleanValue|_|) input =
        match input with
            | 0 -> Some true
            | 1 -> Some false
            | _ -> None

    let IntBoolIdentification value =
        match value with 
            |BooleanValue a -> printfn "The bool value is %b" a
            |_ -> printfn "The value is invalid"

    IntBoolIdentification 0 //The bool value is true
    IntBoolIdentification 1 //The bool value is false
    IntBoolIdentification 2 //The value is invalid

