namespace Cap08

module _04_Type_for_Object_Oriented_Programming =
    
    type CustomerName(firstName : string, middleName, lastName) = 
        member __.FirstName = firstName
        member __.MiddleName = middleName
        member __.LastName = lastName

    type MyClass(intParam : int, strParam : string) = 
        let mutable mutableValue = 42
        member this.SetMutable x = mutableValue <- x
        member this.CurriedAdd x y = x + y
        member this.TupleAdd(x,y) = x + y

    
    type BaseClass(param1) =
        member __.Param1 = param1
    
    type DerivedClass(param1, param2) =
        inherit BaseClass(param1)

    [<AbstractClass>]
    type GeometricBaseClass() =
        abstract member Add: int -> int -> int       // abstract method
        abstract member Pi : float                   // abstract immutable property
        abstract member Area : float with get,set    // abstract read/write property

    //Interface
    type IGeometricBase =
        abstract member Add: int -> int -> int      // abstract method
        abstract member Pi : float                  // abstract immutable property
        abstract member Area : float with get,set   // abstract read/write property
