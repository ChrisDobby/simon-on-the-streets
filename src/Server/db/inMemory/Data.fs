module Db.InMemory.Data

open Domain
open System

let mutable contacts : RegisteredContact list = []

let getContacts () = contacts

let getContact id = 
    let filteredContact contacts =
        match contacts with
            | [] -> None
            | _ -> Some(contacts |> List.exactlyOne)

    filteredContact
        (contacts |> 
                List.filter(fun reg -> reg.Id = id))

let nextId () = 
    match contacts with
        | [] -> 1
        | _ -> (contacts |> List.map(fun reg -> reg.Id) |> List.max) + 1

let addContact (contact: Contact) = 
    let newContact = 
        { 
            Id = nextId()
            Registered = DateTime.Now
            Contact = contact
        }
    contacts <- newContact::contacts

    newContact
