module Styles

open Fable.Helpers.React.Props
open System.Drawing

let fillVertical = Height "100%"

let errorTextColor = Fable.Helpers.React.Props.Color "red"

let flex = [Display "flex"; FlexDirection "column"; Height "100vh"]

let flexFill = [fillVertical; Flex "1"; OverflowX "hidden"; OverflowY "auto"]

let errorMessage = [errorTextColor]

let contactList = [MarginTop "10px"]

let contactCard = [Margin "4px"; Fable.Helpers.React.Props.Color "black"]

let contactIcon = [FontSize "50px"]

let boldText = FontWeight "bold"