namespace Domain

open System

type Contact = 
    {
        Name: string
        Location: string
        Service: string
        SupportOffered: bool
        SupportAccepted: bool
        Intervention: string option
        OtherInformation: string option
    }

type RegisteredContact = 
    {
        Id: int
        Registered: DateTime
        Contact: Contact
    }

type AddContactConfig = 
    {
        PreviousLocations: string list
        Services: string list
    }

module Services = 
    let available = [
        "A & E"
        "Alcohol"
        "Benefits"
        "Court"
        "Dental"
        "Drugs"
        "Education/Training"
        "Family contact"
        "Financial"
        "Food"
        "General support"
        "GP"
        "Hospital visits"
        "Housing"
        "Mental health"
        "Outpatients"
        "Police"
        "Prison visits"
        "Probation"
        "Sexual health"
        "Solicitors"
        "Utilities"
        ]

module Validation =
    let private stringIsFilledIn = function
        | None -> false
        | Some s -> not (String.IsNullOrEmpty(s))

    let validName name (valid, errors) = 
        let nameValid = stringIsFilledIn name
        (valid && nameValid, if nameValid then errors else "Name is required"::errors)

    let validLocation location (valid, errors) = 
        let locationValid = stringIsFilledIn location
        (valid && locationValid, if locationValid then errors else "Location is required"::errors)

    let validService service (valid, errors) = 
        let serviceValid = 
            stringIsFilledIn service &&
            Services.available |> List.map (fun service -> service.ToLower()) |> List.contains(service.Value.ToLower())

        (valid && serviceValid, if serviceValid then errors else "Valid service is required"::errors)

    let validateContact contact =
        (true, []) |>
        validName (Some contact.Name) |>
        validLocation (Some contact.Location) |>
        validService (Some contact.Service)