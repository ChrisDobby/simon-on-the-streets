module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Client.Messages

open Client.Styles

type Model = {
  Page: Page
}

let init () =
  let model = { Page = Page.Home }
  model, Cmd.none

let update msg model =
  model, Cmd.none

let show = function
| Some x -> string x
| None -> "Loading..."

let view model dispatch =
  div [Style flex]
    [ Client.Header.view ]

  
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
