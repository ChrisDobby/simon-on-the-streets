namespace Domain

open System

type Contact = 
    {
        Name: string
        Location: string
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

module Validation =
    let validateContact contact =
        let validName (valid, errors) = 
            let nameValid = not (String.IsNullOrEmpty(contact.Name))
            (valid && nameValid, if nameValid then errors else "Name is required"::errors)

        let validLocation (valid, errors) = 
            let locationValid = not (String.IsNullOrEmpty(contact.Location))
            (valid && locationValid, if locationValid then errors else "Location is required"::errors)

        (true, []) |>
        validName  |>
        validLocation