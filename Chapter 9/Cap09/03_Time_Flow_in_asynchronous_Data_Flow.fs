namespace Cap09

module _03_Time_Flow_in_asynchronous_Data_Flow =
    
    //Buffer with time flow infinite
    let bufferData (number:int) =
        [| for count in 1 .. 10 -> byte (count % 256) |]
        |> Array.permute (fun index -> index)

    let writeFile fileName bufferData =
        async {
          use outputFile = System.IO.File.Create(fileName)
          do! outputFile.AsyncWrite(bufferData) 
        }
    
    Seq.init 10 (fun num -> bufferData num)
    |> Seq.mapi (fun num value -> writeFile ("file" + num.ToString() + ".dat") value)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore


    //Buffer with time flow deterministic
    let bufferData (number:int) =
        [| for i in 1 .. 1000 -> byte (i % 256) |]
        |> Array.permute (fun index -> index)

    let counter = ref 0
    
    let writeFileInner (stream:System.IO.Stream) data =
        let result = stream.AsyncWrite(data)
        lock counter (fun () -> counter := !counter + 1)
        result
    
    let writeFile fileName bufferData =
        async {
          use outputFile = System.IO.File.Create(fileName)
          do! writeFileInner outputFile bufferData
        }
    
    let async1 = Seq.init 1000 (fun num -> bufferData num)
                 |> Seq.mapi (fun num value ->
                     writeFile ("file_timeout" + num.ToString() + ".dat") value)
                 |> Async.Parallel
    try
        Async.RunSynchronously(async1, 200) |> ignore
    with
       | exc -> printfn "%s" exc.Message
                printfn "%d write operations completed successfully." !counter
    



