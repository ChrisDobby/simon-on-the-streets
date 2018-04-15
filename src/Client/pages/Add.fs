module Add

open Domain
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.Import

type Model = {
    LoadingConfig: bool
    Config: AddContactConfig option
    ConfigErrorMessage: string option
    Name: string option
    Location: string option
    Service: string option
    SupportOffered: bool
    SupportAccepted: bool
    Intervention: string option
    OtherInformation: string option
}

type Message =
    | FetchedConfig of AddContactConfig
    | FetchError of exn

let getConfig () =
    promise {
        return! Fetch.fetchAs<AddContactConfig> Urls.APIUrls.AddConfig []
    }

let loadConfigCmd () = 
    Cmd.ofPromise getConfig () FetchedConfig FetchError

let init () = 
    {
        LoadingConfig = true
        Config = None
        ConfigErrorMessage = None
        Name = None
        Location = None
        Service = None
        SupportOffered = false
        SupportAccepted = false
        Intervention = None
        OtherInformation = None
    }, loadConfigCmd ()

let update msg (model: Model) =
    match msg with
        | FetchedConfig config -> 
            { model with Config = Some config; LoadingConfig = false }, Cmd.none
        | FetchError ex -> 
            { model with ConfigErrorMessage = Some ex.Message; LoadingConfig = false }, Cmd.none

let view model dispatch = 
    div [Style Styles.flexFill] [
        div[ClassName "container"] [
            if model.LoadingConfig then
                yield div [ClassName "fa-3x center-align"] [i [ClassName "fas fa-spinner fa-spin"][]]
            else
                yield div [] []
        ]
    ]