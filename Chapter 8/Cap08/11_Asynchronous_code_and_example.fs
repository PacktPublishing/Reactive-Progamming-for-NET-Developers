namespace Cap08

module _11_Asynchronous_code_and_example = 
    
    let tupleByteArray = 
        async { 
            use stream = new System.IO.StreamReader("<filepath>")
            let result1 = stream.BaseStream.AsyncRead(10)
            let! result2 = stream.BaseStream.AsyncRead(10)
            return (Async.RunSynchronously result1), result2
        }

    let item1, item2 = Async.RunSynchronously tupleByteArray

    
    open System.Net

    let urlList = 
        [ "Microsoft.com", "http://www.microsoft.com/"
          "MSDN", "http://msdn.microsoft.com/"
          "Bing", "http://www.bing.com" ]

    let fetchAsync (name, url : string) = 
        async { 
            try 
                let uri = new System.Uri(url)
                let webClient = new WebClient()
                let! html = webClient.AsyncDownloadString(uri)
                printfn "Read %d chars for %s" html.Length name
            with ex -> printfn "%s" (ex.Message)
        }
    
    let runAll() = 
        urlList
        |> Seq.map fetchAsync
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore
    
    runAll() 


    // Try ... with Expression
    let divide2 x y =
        try
          Some( x / y )
        with
          | :? System.DivideByZeroException as ex -> printfn "Exception! %s " (ex.Message); None



