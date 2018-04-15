module Web

open Giraffe
open RequestErrors
open Urls


let App root database =
    let notFound = NOT_FOUND "Page not found"

    let getAllContacts, getContact, addContact = database;

    let x id = Contacts.getContact getContact id

    choose [
            GET >=> choose [
                route PageUrls.Home >=> (htmlFile (System.IO.Path.Combine(root,"index.html")))
                route APIUrls.Contacts >=> (Contacts.getAllContacts getAllContacts)
                routef "/api/contacts/%i" (fun id -> (Contacts.getContact getContact id))
            ]

            POST >=> choose [
                route APIUrls.Contacts >=> (Contacts.register addContact)
            ]

            RequestErrors.notFound (text "Not Found")
    ]