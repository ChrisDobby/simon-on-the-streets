namespace Domain

open System

type Contact = 
    {
        Name: string
        Location: string
        Service: string
        SupportOffered: bool
        SupportAccepted: bool
        Intervention: string
        OtherInformation: string
    }

type RegisteredContact = 
    {
        Id: int
        Registered: DateTime
        Contact: Contact
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
    let validateContact contact =
        let validName (valid, errors) = 
            let nameValid = not (String.IsNullOrEmpty(contact.Name))
            (valid && nameValid, if nameValid then errors else "Name is required"::errors)

        let validLocation (valid, errors) = 
            let locationValid = not (String.IsNullOrEmpty(contact.Location))
            (valid && locationValid, if locationValid then errors else "Location is required"::errors)

        let validService (valid, errors) = 
            let serviceValid = 
                not (String.IsNullOrEmpty(contact.Service)) &&
                Services.available |> List.contains(contact.Service)

            (valid && serviceValid, if serviceValid then errors else "Valid service is required"::errors)

        (true, []) |>
        validName  |>
        validLocation |>
        validService