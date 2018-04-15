module Views

type PageModel = 
    | HomePage of Home.Model

type Model = {
    PageModel: PageModel
}

type Msg =
    | HomePageMsg of Home.Message

open Fable.Helpers.React
open Fable.Helpers.React.Props

let viewPage model dispatch =
    match model.PageModel with 
        | HomePage m -> Home.view m dispatch

let view model dispatch = 
    div [Style Styles.flex]
        [ 
            Header.view 
            viewPage model dispatch
        ]
