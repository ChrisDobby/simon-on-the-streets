module Views

type PageModel = 
    | HomePage

type Model = {
    PageModel: PageModel
}

open Fable.Helpers.React
open Fable.Helpers.React.Props

let viewPage model dispatch =
    match model.PageModel with 
        | HomePage -> Home.view ()

let view model dispatch = 
  div [Style Styles.flex]
    [ 
        Header.view 
        viewPage model dispatch
    ]
