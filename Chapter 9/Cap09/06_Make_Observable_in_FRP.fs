namespace Cap09

module _06_Make_Observable_in_FRP =
    
    module Observable =
        open System
        let ofSeq (values:'T seq) : IObservable<'T> =
            { 
            new IObservable<'T> with
               member __.Subscribe(obs) =
                  for x in values do obs.OnNext(x)
                  { 
                  new IDisposable with 
                      member __.Dispose() = ()
                  }
            }
        let inline print (observable:IObservable< ^T >) : IObservable< ^T > =
            { 
            new IObservable<'T> with 
               member this.Subscribe(observer:IObserver<'T>) =
                  let value = ref (Unchecked.defaultof<'T>)
                  let iterator =
                     { 
                     new IObserver<'T> with 
                        member __.OnNext(x) = printfn "%A" x 
                        member __.OnCompleted() = observer.OnNext(!value)
                        member __.OnError(_) = failwith "Error"
                     }
                  observable.Subscribe(iterator)
            }
        let first (obs:IObservable<'T>) : 'T =
            let value = ref (Unchecked.defaultof<'T>)
            let _ = obs.Subscribe(fun x -> value := x)
            !value


    let obsValue =
        {0.0..100.0}
        |> Observable.ofSeq
        |> Observable.filter (fun x -> x % 2.0 = 0.0)
        |> Observable.map (fun x -> x ** 3.0)
        |> Observable.print
        |> Observable.first


