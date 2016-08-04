namespace Cap08

module _03_Type_inference =
    
    //declare a tuple
    let tupleValues = 1, "one"
    
    //unpack values
    let v1, v2 = tupleValues
    
    let sumfloat values = List.reduce (+) values
    printfn "The sum is: %A" (sumfloat [4.6; 10.3])


//Result in Interactive

//The sum is: 14.9

//val tupleValues : int * string = (1, "one")
//val v2 : string = "one"
//val v1 : int = 1
//val sumfloat : values:float list -> float
//val it : unit = ()