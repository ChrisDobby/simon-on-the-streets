module ContactList

open Fable.Core
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Domain

type [<Pojo>] ContactProps = { key: int; reg: RegisteredContact }
let contactCard { reg = reg }  = 
    div[ClassName "card col s12 m6 l4 xl3"; Style Styles.contactCard] [
        div [ClassName "card-content"] [
            span [ClassName "card-title"] [str reg.Contact.Name]
            h6 [] [str reg.Contact.Location]
            h6 [] [str reg.Contact.Service]
            str (reg.Registered.ToString("dd-MMM-yyyy hh:mm"))
            ]
        ]

let inline ContactComponent props = (ofFunction contactCard) props []

let view contacts =
    match contacts with
        | [] -> div [ClassName "row"]
                    [h5 [ClassName "center-align blue-text text-darken-2"] [str "No contacts found"]]
        | _ ->
            div [ClassName "row"] [
                contacts |> List.map(fun contact -> 
                        ContactComponent {
                            key = contact.Id
                            reg = contact
                             })
                        |> ofList
    ]