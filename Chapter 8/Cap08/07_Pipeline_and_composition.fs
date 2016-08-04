namespace Cap08 
    
module _07_Pipeline_and_composition =
    
    //Pipeline
    [ 0..10 ] |> List.iter (fun x -> printfn "the value is %d" x)
    List.iter (fun x -> printfn "the value is %d" x) <| [ 0..10]

    //Composition
    let add1 x = x + 1
    let times2 x = 2 * x

    let Compose2 = add1 >> times2 //Forward
    let Compose1 = add1 << times2 //Backward

    let res1 = Compose2 1 //The result is 4
    let res2 = Compose1 1 //The result is 4

    //Write as
    [ 0..10 ]
        |> List.map (fun x -> x * x)
        |> List.iter (fun y -> printfn "the value is: %i " y)

    //or

    [ 0..10 ] |> List.map (fun x -> x * x) |> List.iter (fun y -> printfn "the value is: %i " y)
