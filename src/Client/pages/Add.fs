module Add

open Domain
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.Import

type Model = {
    LoadingConfig: bool
    Name: string
    Location: string
    Service: string
    SupportOffered: bool
    SupportAccepted: bool
    Intervention: string option
    OtherInformation: string option
}

type Message =
    | FetchedConfig

let init () = 
    {
        LoadingConfig = true
        Name = ""
        Location = ""
        Service = ""
        SupportOffered = false
        SupportAccepted = false
        Intervention = None
        OtherInformation = None
    }, Cmd.none

let update msg (model: Model) =
    model, Cmd.none

let view model dispatch = 
    div [Style Styles.flexFill] [
        div[ClassName "container"] [
            if model.LoadingConfig then
                yield div [ClassName "fa-3x center-align"] [i [ClassName "fas fa-spinner fa-spin"][]]
            else
                yield div [] []
        ]
    ]