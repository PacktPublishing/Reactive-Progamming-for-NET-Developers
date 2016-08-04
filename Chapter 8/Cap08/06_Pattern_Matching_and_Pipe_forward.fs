namespace Cap08

module _06_Pattern_Matching_and_Pipe_forward =
    let v1 = 3
    let v2 = 2

    let printEvenOdd x =
        match x % 2 with
        | 0 -> printfn "The value is odd"
        | _ -> printfn "The value is even" 
    
    printEvenOdd v1
    printEvenOdd v2

    let compareTwoValue x = 
        match x with
            | (v1, v2) when v1 > v2 -> printfn "%d is greater than %d" v1 v2
            | (v1, v2) when v1 < v2 -> printfn "%d is less than %d" v1 v2
            | (v1, v2) -> printfn "%d equals %d" v1 v2

    compareTwoValue (0, 1)
    compareTwoValue (1, 0)
    compareTwoValue (0, 0)




