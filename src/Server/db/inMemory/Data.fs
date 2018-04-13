module Db.InMemory.Data

open Domain
open System

let mutable registrations : Registration list = []

let getRegistrations () = registrations

let getRegistration id = 
    let filteredRegistration registrations =
        match registrations with
            | [] -> None
            | _ -> Some(registrations |> List.exactlyOne)

    filteredRegistration
        (registrations |> 
                List.filter(fun reg -> reg.Id = id))

let nextId () = 
    match registrations with
        | [] -> 1
        | _ -> (registrations |> List.map(fun reg -> reg.Id) |> List.max) + 1

let addRegistration (registration: RegistrationRequest) = 
    let newRegistration = { Id = nextId(); Name = registration.Name; Registered = DateTime.Now }
    registrations <- newRegistration::registrations

    newRegistration
