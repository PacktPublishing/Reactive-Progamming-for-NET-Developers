namespace Cap08

module _12_What_is_FRP_and_how_is_it_represented =
    
    let numberList = [ 2.0; 4.0; 6.0] 
    let result = numberList |> List.map (fun x -> x**2.0)


