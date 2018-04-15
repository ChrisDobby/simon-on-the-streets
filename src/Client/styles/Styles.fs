module Styles

open Fable.Helpers.React.Props
open System.Drawing

let fillVertical = Height "100%"

let errorTextColor = Fable.Helpers.React.Props.Color "red"

let flex = [Display "flex"; FlexDirection "column"; Height "100vh"]

let flexFill = [fillVertical; Flex "1"; OverflowX "hidden"; OverflowY "auto"]

let errorMessage = [errorTextColor]

let contactCard = [Margin "4px"]