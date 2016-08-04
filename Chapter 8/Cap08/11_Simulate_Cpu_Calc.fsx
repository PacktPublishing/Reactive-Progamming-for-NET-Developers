// Simulate CPU calc. 
let child() = 
    for i in [ 1..4000 ] do
        for i in [ 1..400 ] do
            do "Fsharp".Contains("F") |> ignore

let parentTask = 
    child
    |> List.replicate 30
    |> List.reduce (>>)

let asyncChild = async { return child() }

let asyncParentTask = 
    asyncChild
    |> List.replicate 30
    |> Async.Parallel
    

//test single
#time 
parentTask()
#time 

//test async with parallel
#time 
asyncParentTask |> Async.RunSynchronously
#time

