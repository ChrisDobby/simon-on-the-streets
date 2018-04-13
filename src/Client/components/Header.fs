module Client.Header

open Fable.Helpers.React
open Fable.Helpers.React.Props

let view = 
    nav [ClassName "teal lighten-2"] 
      [
        div [ClassName "nav-wrapper"] 
          [        
            a [ClassName "brand-logo"; Href "#"] [img [Src "/images/simon-header.png"]]
          ]
      ]
