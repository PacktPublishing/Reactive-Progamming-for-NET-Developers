namespace Cap08

module _08_Discriminate_Unions_and_Record_Type =
    
    //Syntax of Record Type
    
    //[ attributes ]
    //type [accessibility-modifier] typename = { 
    //    [ mutable ] label1 : type1;
    //    [ mutable ] label2 : type2;
    //    ...
    //    }
    //    member-list

    //Record Type
    type Point = { x : float; y: float; z: float option; } 

    let point2D = { x = 1.0; y = 1.0; z = None}
    let point3D = { point2D with z = Some(-1.0)}


    //Syntax of Discriminated Unions
    
    // type type-name =
    //     | case-identifier1 [of [ fieldname1 : ] type1 [ * [ fieldname2 : ] type2 ...]
    //     | case-identifier2 [of [fieldname3 : ]type3 [ * [ fieldname4 : ]type4 ...]
    //     ...

    //Discriminated unions
    type Shape =
        | Rectangle of width : float * length : float
        | Circle of radius : float
        | Prism of width : float * float * height : float
    
    let rect = Rectangle(length = 1.3, width = 10.0)
    let circ = Circle (1.0)
    let prism = Prism(5., 2.0, height = 3.0)


    //This code is derived from Tutorial F# project of Microsoft
    type Suit = 
    | Hearts 
    | Clubs 
    | Diamonds
    | Spades
        // Represents the rank of a playing card
    type Rank = 
        | Value of int // Represents the rank of cards 2 .. 10
        | Ace
        | King
        | Queen
        | Jack
        static member GetAllRanks() = 
            [ yield Ace
              for i in 2 .. 10 do yield Value i
              yield Jack
              yield Queen
              yield King ]
                                   
    type Card =  { Suit: Suit; Rank: Rank }
              
    // Returns a list representing all the cards in the deck
    let fullDeck = 
        [ for suit in [ Hearts; Diamonds; Clubs; Spades] do
              for rank in Rank.GetAllRanks() do 
                  yield { Suit=suit; Rank=rank } ]
