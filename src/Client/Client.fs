module Client

open Elmish
open Elmish.Browser.Navigation
open Elmish.React

open Pages
open Views

let handleNotFound (model: Model) =
    ( model, Navigation.modifyUrl (toPath Page.Home) )

let urlUpdate (result:Page option) (model: Model) =
    match result with
    | None ->
        handleNotFound model

    | Some Home ->
        { model with PageModel = HomePage }, Cmd.none

let init result =
  match result with
    | None -> { PageModel = HomePage }, Cmd.none
    | Some Home -> { PageModel = HomePage }, Cmd.none

let update msg model =
  model, Cmd.none
  
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
|> Program.toNavigable urlParser urlUpdate
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
