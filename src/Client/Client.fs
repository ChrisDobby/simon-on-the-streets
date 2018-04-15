module Client

open Elmish
open Elmish.Browser.Navigation
open Elmish.React

open Pages
open Views

let handleNotFound (model: Model) =
    ( model, Navigation.modifyUrl (toPath Page.Home) )

let homePage () =
    let m, cmd = Home.init ()
    { PageModel = HomePage m }, Cmd.map HomePageMsg cmd

let urlUpdate (result:Page option) (model: Model) =
    match result with
    | None ->
        handleNotFound model

    | Some Home ->
        let m, cmd = Home.init ()
        { model with PageModel = HomePage m }, Cmd.map HomePageMsg cmd

    | Some AddContact ->
        let m, cmd = Add.init ()
        { model with PageModel = AddContactPage m }, Cmd.map AddPageMsg cmd

let init result =
  match result with
    | None -> homePage()
    | Some Home -> homePage()
    | Some AddContact -> 
        let m, cmd = Add.init ()
        { PageModel = AddContactPage m }, Cmd.map AddPageMsg cmd

let update msg model =
    match msg, model.PageModel with
        | HomePageMsg msg, HomePage m -> 
            let m, cmd = Home.update msg m
            { model with PageModel = HomePage m }, Cmd.map HomePageMsg cmd
        | AddPageMsg msg, AddContactPage m ->
            let m, cmd = Add.update msg m
            { model with PageModel = AddContactPage m }, Cmd.map AddPageMsg cmd
        | NewContact, _ ->
            model, Navigation.newUrl (toPath Page.AddContact)
        | _, _ -> model, Cmd.none
  
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
