namespace Cap08

module _02_Immutable_and_deduce_type =
    
    //C# code
    //int x = 5;
    //x = x + 1;

    //F# code:
    let x = 5
    x <- x + 1 //error FS0027: This value is not mutable. 

    let mutable x = 6
    x <- x + 1
