module ContactList

open Fable.Core
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Domain

type [<Pojo>] ContactProps = { key: int; reg: RegisteredContact }
let contactCard { reg = reg }  = 
    Materialize.initialiseModal (sprintf "#infoModal%d" reg.Id)

    let supportStr = 
        match reg.Contact.SupportOffered with
         | true -> sprintf "Support offered and %s" (if reg.Contact.SupportAccepted then "accepted" else "declined")
         | _ -> "Support not offered"

    div[] [
        a[ClassName "card col s12 m6 l4 xl3 light-blue lighten-5 modal-trigger"; Style Styles.contactCard; Href (sprintf "#infoModal%d" reg.Id)] [
            div [ClassName "card-content"] [
                div[ClassName "row"] [
                    div[ClassName "col s10"] [
                        span [ClassName "card-title"] [str reg.Contact.Name]
                        h6 [] [str reg.Contact.Location]
                        h6 [] [str reg.Contact.Service]
                        div [] [str (reg.Registered.ToString("dd-MMM-yyyy hh:mm"))]
                        ]
                    div [ClassName "col s2"] [i [ClassName "far fa-user"; Style Styles.contactIcon] []]
                    div [ClassName "col s12"] [str supportStr]
                    ]
                ]
            ]
        div [Id (sprintf "infoModal%d" reg.Id); ClassName "modal"] [
            div [ClassName "modal-content"] [
                h4[] [str reg.Contact.Name]
                h5[] [str (sprintf "Location: %s" reg.Contact.Location)]
                h5[] [str (sprintf "Service: %s" reg.Contact.Service)]
                h5[] [str (sprintf "Date: %s" (reg.Registered.ToString("dd-MMM-yyyy hh:mm")))]
                h5[] [str supportStr]                
                h5[Style [Styles.boldText]] [str "Intervention:"]
                (match reg.Contact.Intervention with
                    | Some intervention -> h5 [] [str intervention]
                    | _ -> h5 [] [str "None recorded"])
                h5[Style [Styles.boldText]] [str "Other information:"]
                (match reg.Contact.OtherInformation with
                    | Some info -> h5 [] [str info]
                    | _ -> h5 [] [str "None recorded"])
                ]
            ]
    ]

let inline ContactComponent props = (ofFunction contactCard) props []

let view contacts =
    match contacts with
        | [] -> div [ClassName "row"]
                    [h5 [ClassName "center-align blue-text text-darken-2"] [str "No contacts found"]]
        | _ ->
            div [ClassName "row"; Style Styles.contactList] [
                contacts |> List.map(fun contact -> 
                        ContactComponent {
                            key = contact.Id
                            reg = contact
                             })
                        |> ofList
    ]