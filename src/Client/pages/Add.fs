module Add

open Domain
open Elmish
open Elmish.Browser.Navigation
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Fable.PowerPack
open Fable.Import
open System
open Fable.PowerPack.Fetch.Fetch_types
open System.Net.Http
open System.Net.Http.Headers

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
    NameError: bool * string list
    LocationError: bool * string list
    ServiceError: bool * string list
    CanSave: bool
    Saving: bool
    SaveErrorMessage: string option
}

type Message =
    | FetchedConfig of AddContactConfig
    | FetchError of exn
    | SavedContact of RegisteredContact
    | SaveError of exn
    | NameChanged of string
    | LocationChanged of string
    | ServiceChanged of string
    | SupportOfferedChanged of bool
    | SupportAcceptedChanged of bool
    | InterventionChanged of string
    | OtherInformationChanged of string
    | SaveContact

let getConfig () =
    promise {
        return! Fetch.fetchAs<AddContactConfig> Urls.APIUrls.AddConfig []
    }

let postContact contact = 
    promise {
        let body = toJson contact
        let props = 
            [ 
                RequestProperties.Method HttpMethod.POST
                Fetch.requestHeaders [ HttpRequestHeaders.ContentType "application/json" ]
                RequestProperties.Body !^body
            ]

        return! Fetch.fetchAs<RegisteredContact> Urls.APIUrls.Contacts props
    }

let loadConfigCmd () = 
    Cmd.ofPromise getConfig () FetchedConfig FetchError

let postContactCmd contact =
    Cmd.ofPromise postContact contact SavedContact SaveError

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
        NameError = (true, [])
        LocationError = (true, [])
        ServiceError = (true, [])
        CanSave = false
        Saving = false
        SaveErrorMessage = None
    }, loadConfigCmd ()

let validationCssClass error = 
    let valid, _ = error
    if valid then "valid" else "invalid"

let serviceOptions config =
    option [HTMLAttr.Value ""; HTMLAttr.Disabled true; HTMLAttr.Selected true][str "Select a service..."]::
    (config.Services |> List.map (fun service -> option [HTMLAttr.Value service][str service]))

let addForm model dispatch =
    Materialize.autoInit()
    Materialize.initialiseAutocompleteFromList "#loc" model.Config.Value.PreviousLocations

    div [] [
        (if model.Saving then
            div [ClassName "fa-3x center-align"] [i [ClassName "fas fa-spinner fa-spin"][]]
        else
            h4[] [str "New contact"])
        div [ClassName "col s12"] [
            div [ClassName "row"] [
                div [ClassName "input-field"] [
                    input [
                        Id "name"
                        HTMLAttr.Type "text"
                        ClassName (validationCssClass model.NameError)
                        OnChange (fun (ev:React.FormEvent) -> dispatch (NameChanged !!ev.target?value)) 
                        ]                    
                    label [HtmlFor "name"] [str "Name"]
                    ]
                ]
            div [ClassName "row"] [
                div [ClassName "input-field"] [
                    input [
                        Id "loc"
                        HTMLAttr.Type "text"
                        ClassName (sprintf "autocomplete %s" (validationCssClass model.LocationError))
                        OnChange (fun (ev:React.FormEvent) -> dispatch (LocationChanged !!ev.target?value)) 
                        ]
                    label [HtmlFor "loc"] [str "Location"]
                    ]
                ]
            div [ClassName "row"] [
                div [ClassName "input-field"] [
                    select [
                        Id "service"
                        OnChange (fun (ev:React.FormEvent) -> dispatch (ServiceChanged !!ev.target?value)) 
                        ] 
                        (serviceOptions (model.Config.Value))
                    label [HtmlFor "service"] [str "Service"]
                    ]
                ]
            div [ClassName "row"] [
                div[ClassName "input-field"] [
                    span [] [str "Support offered"]
                    div [ClassName "switch"] [
                        label [] [
                            str "No"
                            input[
                                HTMLAttr.Type "checkbox"
                                OnChange (fun (ev:React.FormEvent) -> dispatch (SupportOfferedChanged !!ev.target?``checked``))
                                ]
                            span[ClassName "lever"][]
                            str "Yes"
                            ]
                        ]
                    ]
                ]

            (if model.SupportOffered then
                div [ClassName "row"] [
                    div[ClassName "input-field"] [
                        span [] [str "Support accepted"]
                        div [ClassName "switch"] [
                            label [] [
                                str "No"
                                input[
                                    HTMLAttr.Type "checkbox"
                                    OnChange (fun (ev:React.FormEvent) -> dispatch (SupportAcceptedChanged !!ev.target?``checked``))
                                    ]
                                span[ClassName "lever"][]
                                str "Yes"
                                ]
                            ]
                        ]
                    ]
            else
                div [] [])
            div [ClassName "row"] [
                div [ClassName "input-field"] [
                    textarea [
                        Id "intervention"
                        ClassName "materialize-textarea"
                        OnChange (fun (ev:React.FormEvent) -> dispatch (InterventionChanged !!ev.target?value)) 
                        ] []
                    label [HtmlFor "intervention"] [str "Intervention"]
                    ]
                ]
            div [ClassName "row"] [
                div [ClassName "input-field"] [
                    textarea [
                        Id "otherInformation"
                        ClassName "materialize-textarea"
                        OnChange (fun (ev:React.FormEvent) -> dispatch (OtherInformationChanged !!ev.target?value)) 
                        ] []
                    label [HtmlFor "otherInformation"] [str "Other information"]
                    ]
                ]
            div [ClassName "row"] [
                button [
                    ClassName "waves-effect waves-light btn"
                    HTMLAttr.Disabled (not (model.CanSave))
                    OnClick (fun _ -> dispatch SaveContact)
                    ] [str "Save contact"]
                ]
            ]
        ]

let update msg (model: Model) =
    let newString str = if str = "" then None else Some str

    let modelWithCanSave m = 
        let valid, _ = (
            (true, []) |> 
            Validation.validName m.Name |> 
            Validation.validLocation m.Location |> 
            Validation.validService m.Service)
        { m with CanSave = valid }

    match msg with
        | FetchedConfig config -> 
            { model with Config = Some config; LoadingConfig = false }, Cmd.none
        | FetchError ex -> 
            { model with ConfigErrorMessage = Some ex.Message; LoadingConfig = false }, Cmd.none
        | NameChanged name ->
            let newName = newString name
            let newModel = { model with Name = newName; NameError = Validation.validName newName (true, []) }
            modelWithCanSave newModel, Cmd.none
        | LocationChanged location ->
            let newLocation = newString location
            let newModel = { model with Location = newLocation; LocationError = Validation.validLocation newLocation (true, []) }
            modelWithCanSave newModel, Cmd.none
        | ServiceChanged service ->
            let newService = newString service
            let newModel = { model with Service = newService; ServiceError = Validation.validService newService (true, []) }
            modelWithCanSave newModel, Cmd.none
        | InterventionChanged intervention ->
            { model with Intervention = newString intervention}, Cmd.none
        | OtherInformationChanged otherInformation ->
            { model with OtherInformation = newString otherInformation}, Cmd.none
        | SupportOfferedChanged offered ->
            { model with SupportOffered = offered }, Cmd.none
        | SupportAcceptedChanged accepted ->
            { model with SupportAccepted = accepted }, Cmd.none
        | SaveContact ->
            { model with Saving = true }, postContactCmd(
                {
                    Name = model.Name.Value
                    Location = model.Location.Value
                    Service = model.Service.Value
                    SupportOffered = model.SupportOffered
                    SupportAccepted = model.SupportAccepted
                    Intervention = model.Intervention
                    OtherInformation = model.OtherInformation
                })
        | SaveError ex -> { model with Saving = false; SaveErrorMessage = Some ex.Message }, Cmd.none
        | SavedContact _ -> model, Navigation.newUrl (Pages.toPath Pages.Home)

let view model (dispatch: Message -> unit) = 
    div [Style Styles.flexFill] [
        div[ClassName "container"] [
            if model.LoadingConfig then
                yield div [ClassName "fa-3x center-align"] [i [ClassName "fas fa-spinner fa-spin"][]]
            else
                yield addForm model dispatch
        ]
    ]