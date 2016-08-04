namespace Cap08

module _10_The_concept_of_Workflow_Asynchronous =
    
    open System.IO

    let fileContent (path : string) = 
        async { 
            use stream = new StreamReader(path)
            return stream.ReadToEnd()
        }
    
    let res = Async.RunSynchronously(fileContent "<filepath>")

