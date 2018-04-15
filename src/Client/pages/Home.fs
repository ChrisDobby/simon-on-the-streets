module Home

open Domain
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Elmish.Browser.Navigation

type Model = {
    Contacts: RegisteredContact list
    LoadingContacts: bool
    ErrorMessage: string option
}

type Message = 
    | FetchedContacts of RegisteredContact list
    | FetchError of exn


let getContacts () =
    promise {
        return! Fetch.fetchAs<RegisteredContact list> Urls.APIUrls.Contacts []
    }
    
let addContactClick _ =
    Navigation.newUrl (Pages.toPath Pages.AddContact) |> List.map (fun f -> f ignore) |> ignore

let loadContactsCmd () = 
    Cmd.ofPromise getContacts () FetchedContacts FetchError

let init () = 
    {
        Contacts = []
        LoadingContacts = true
        ErrorMessage = None
    }, loadContactsCmd ()

let update msg (model: Model) =
    match msg with
        | FetchedContacts contacts -> 
            { model with Contacts = contacts; LoadingContacts = false }, Cmd.none
        | FetchError ex -> 
            { model with ErrorMessage = Some ex.Message; LoadingContacts = false }, Cmd.none

let view model dispatch = 
    div [Style Styles.flexFill] [
        div[ClassName "container"] [
            if model.LoadingContacts then
                yield div [ClassName "fa-3x center-align"] [i [ClassName "fas fa-spinner fa-spin"][]]
            else
                match model.ErrorMessage with
                    | Some error -> 
                        yield div [Style Styles.errorMessage] [
                            h5 [ClassName "center-align"] [str "The following error occurred:"]
                            h6 [ClassName "center-align"] [str error]
                            h5 [ClassName "center-align"] [str "Refresh to try again."]
                            ]
                    | _ -> yield ContactList.view model.Contacts
                ]
        div[ClassName "fixed-action-btn"] [
            button [ClassName "btn-floating btn-large green"; Href (Pages.toPath Pages.AddContact); OnClick addContactClick] [
                i [ClassName "fa fa-plus"] []
                ]
            ]
    ]