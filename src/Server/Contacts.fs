module Contacts

open Giraffe
open ServerErrors
open Domain
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open System.Threading.Tasks

let getAllContacts (getContactsFromDB: unit -> Task<Contact list>) next (ctx: HttpContext) =
    task {
        try
            let! contacts = getContactsFromDB ()
            return! Successful.OK (contacts) next ctx
        with exn ->
            let logger = ctx.GetLogger "Contacts"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx
    }

let getContact (getContactFromDb: int -> Task<Option<Contact>>) id next (ctx: HttpContext) = 
    task {
        try
            let! contact = getContactFromDb(id)

            match contact with
                | Some(reg) -> return! Successful.OK (reg) next ctx
                | _ -> return! RequestErrors.NOT_FOUND "contact not found" next ctx

        with exn ->
            let logger = ctx.GetLogger "Contacts"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx        
    }

let register (addContact: ContactRequest -> Task<Contact>) next (ctx: HttpContext) =
    task {
        try
            let! registerRequest = ctx.BindJsonAsync<ContactRequest>()

            let! newContact = addContact registerRequest
            return! Successful.OK(newContact) next ctx
        with exn ->
            let logger = ctx.GetLogger "Contacts"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx
    }