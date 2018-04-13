module Web

open Giraffe
open RequestErrors

let inMemoryDataFunctions =
    (fun () -> task { return Db.InMemory.Data.getRegistrations () }),
    (fun id -> task { return Db.InMemory.Data.getRegistration id }),
    (fun reg -> task { return Db.InMemory.Data.addRegistration reg })

let App root =
    let notFound = NOT_FOUND "Page not found"

    let getAllRegistrations, getRegistration, addRegistration = inMemoryDataFunctions;

    let x id = Registrations.getRegistration getRegistration id

    choose [
            GET >=> choose [
                route "/" >=> (htmlFile (System.IO.Path.Combine(root,"index.html")))
                route "/api/registrations" >=> (Registrations.getAllRegistrations getAllRegistrations)
                routef "/api/registrations/%i" (fun id -> (Registrations.getRegistration getRegistration id))
            ]

            POST >=> choose [
                route "/api/registrations" >=> (Registrations.register addRegistration)
            ]

            RequestErrors.notFound (text "Not Found")
    ]