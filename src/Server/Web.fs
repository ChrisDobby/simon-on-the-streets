module Web

open Giraffe
open Urls
open Microsoft.AspNetCore.Http


let App root database =
    let getAllContacts, getContact, addContact, getPreviousLocations = database;

    let apiPathPrefix = PathString("/api")
    let notfound: HttpHandler =
        fun next ctx ->
            if ctx.Request.Path.StartsWithSegments(apiPathPrefix) then
                RequestErrors.NOT_FOUND "Page not found" next ctx
            else
                (htmlFile (System.IO.Path.Combine(root,"index.html"))) next ctx

    choose [
            GET >=> choose [
                route PageUrls.Home >=> (htmlFile (System.IO.Path.Combine(root,"index.html")))
                route APIUrls.Contacts >=> (Contacts.getAllContacts getAllContacts)
                routef "/api/contacts/%i" (fun id -> (Contacts.getContact getContact id))
                route APIUrls.AddConfig >=> (Contacts.getAddConfig getPreviousLocations)
            ]

            POST >=> choose [
                route APIUrls.Contacts >=> (Contacts.register addContact)
            ]

            notfound
    ]