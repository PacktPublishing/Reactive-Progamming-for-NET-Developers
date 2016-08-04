namespace Cap08

module _14_Example_of_Scenario_with_AsyncSeq =
    #r @"../packages/FSharp.Control.AsyncSeq.2.0.8/lib/net45/FSharp.Control.AsyncSeq.dll"
    
    open FSharp.Control
    
    let asyncSeq = asyncSeq { 
       do! Async.Sleep(100)
       yield 1
       do! Async.Sleep(100)
       yield 2 }
    
    let two = asyncSeq |> AsyncSeq.filter (fun x -> x = 2) |> AsyncSeq.iter (fun x -> printfn "%i" x)
    
    let res = two |> Async.RunSynchronously
    
    let syncSeq = seq {
      System.Threading.Thread.Sleep(100) 
      yield 1
      System.Threading.Thread.Sleep(100) 
      yield 2
    }

