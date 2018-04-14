module Web

open Giraffe
open RequestErrors


let App root database =
    let notFound = NOT_FOUND "Page not found"

    let getAllContacts, getContact, addContact = database;

    let x id = Contacts.getContact getContact id

    choose [
            GET >=> choose [
                route "/" >=> (htmlFile (System.IO.Path.Combine(root,"index.html")))
                route "/api/contacts" >=> (Contacts.getAllContacts getAllContacts)
                routef "/api/contacts/%i" (fun id -> (Contacts.getContact getContact id))
            ]

            POST >=> choose [
                route "/api/contacts" >=> (Contacts.register addContact)
            ]

            RequestErrors.notFound (text "Not Found")
    ]