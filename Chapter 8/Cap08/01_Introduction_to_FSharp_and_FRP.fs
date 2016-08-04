namespace Cap08

module _01_Introduction_to_FSharp_and_FRP =
    let sum a b = a + b
    let add5 = sum 5
    let result = add5 2 //The result is value 7
    let result2 = add5 3 //The result is value 8

    [0..100] |> List.map (fun x -> x * x) 
       |> List.iter (fun y -> printfn "the value is: %i " y)

