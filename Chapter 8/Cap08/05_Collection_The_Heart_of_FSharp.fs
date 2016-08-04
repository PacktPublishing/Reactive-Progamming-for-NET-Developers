namespace Cap08

module _05_Collection_The_Heart_of_FSharp =
    
    module Array =
        let myArr1 = [| 'a'; 'b'; 'c' |]
        let myArr2 = [| 1 .. 5 |]
        let myArr3 = [| for i in 1 .. 5 -> i * i |]
        let arrayOfZeroes : int array = Array.zeroCreate 5 
        let multiArr = [| [|0,0|]; [|1,1|] |]

        let arr = [|0..4..16|]  //val arr : int [] = [|0; 2; 4; 6; 8; 10; 12; 14; 16|]

        let a = myArr2.[1]      // 2
        let b = myArr2.[2..]    // 3,4,5
        let c = myArr2.[..2]    // 1,2,3
        let d = multiArr.[1]    // [|(1, 1)|]

        myArr2.[1] <- 3         // to change a value

    
    module List =
        let myList2 = [ 1 .. 5 ]
        let myList3 = [ for i in 1 .. 5 -> i * i ]
        let list = [0..2..16]

        let a = myList2.[1]     // 2
        let b = myList2.[2..]   // 3,4,5
        let c = myList2.[..2]   // 1,2,3

    module Seq =
        let myseq1 = seq { 0 .. 10 .. 100 }
        let myseq2 = seq { for i in 1 .. 5 do yield i * i }
        let myList3 = Seq.init 5 (fun n -> n * 5)

    [| 1 .. 100 |] 
        |> Seq.ofArray 
        |> Seq.map (fun x -> x * x) 
        |> List.ofSeq
        |> List.iter (fun y -> printfn "the value is: %i " y)

