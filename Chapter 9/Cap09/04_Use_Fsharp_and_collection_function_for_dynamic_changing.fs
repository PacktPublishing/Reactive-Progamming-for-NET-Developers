namespace Cap09

module _04_Use_Fsharp_and_collection_function_for_dynamic_changing =
    
    //Syntax Expression
    type Result<'TSuccess,'TFailure> = 
        | Success of 'TSuccess
        | Failure of 'TFailure

    let bind inputFunc = 
        function
            | Success s -> inputFunc s
            | Failure f -> Failure f


    type Account = { UserName : string; IsLogged : bool; Email : string  }
    
    let validateAccount account =
        match account with
            | account when account.UserName = "" -> Failure "UserName is not valid"
            | account when account.Email = "" -> Failure " Email is not empty"
            | _ -> Success account
    
    
    let checkLogin account =
        if(account.IsLogged) then
            Success account
        else
            Failure "User is not logged"
    
    let LogIn account =
        if(account.IsLogged) then
            Failure "User has already Logged"
        else
            Success {account with IsLogged = true}
    
    let LogOut account =
        if(account.IsLogged) then
            Success {account with IsLogged = false}
        else
            Failure "User has already Logged"
    
    let ProcessNewAccount = 
        let checkLogin = bind checkLogin
        let login = bind LogIn
        validateAccount >> login >> checkLogin
    
    let NewFakeAccount = { UserName = ""; Email = ""; IsLogged = false }
    let AccountLogged = { UserName = "User"; Email = "user@user.net"; IsLogged = true }
    let NewAccount = { UserName = "User1"; Email = " user1@user.net "; IsLogged = false }
    
    ProcessNewAccount NewFakeAccount |> printfn "Result = %A"
    ProcessNewAccount AccountLogged |> printfn "Result = %A"
    ProcessNewAccount NewAccount |> printfn "Result = %A"


