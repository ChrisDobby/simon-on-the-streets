module Contacts

open Giraffe
open ServerErrors
open Domain
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open System.Threading.Tasks

let getAllContacts (getContactsFromDB: unit -> Task<RegisteredContact list>) next (ctx: HttpContext) =
    task {
        try
            let! contacts = getContactsFromDB ()
            return! Successful.OK(ctx.WriteJsonAsync contacts) next ctx
        with exn ->
            let logger = ctx.GetLogger "Contacts"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx
    }

let getContact (getContactFromDb: int -> Task<Option<RegisteredContact>>) id next (ctx: HttpContext) = 
    task {
        try
            let! contact = getContactFromDb(id)

            match contact with
                | Some(reg) -> return! Successful.OK(ctx.WriteJsonAsync reg) next ctx
                | _ -> return! RequestErrors.NOT_FOUND "contact not found" next ctx

        with exn ->
            let logger = ctx.GetLogger "Contacts"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx        
    }

let register (addContact: Contact -> Task<RegisteredContact>) next (ctx: HttpContext) =
    task {
        try
            let! registerRequest = ctx.BindJsonAsync<Contact>()

            let validationResult, errors = Validation.validateContact registerRequest

            match validationResult with
                | true ->
                    let! newContact = addContact registerRequest
                    return! Successful.OK(ctx.WriteJsonAsync newContact) next ctx
                | false -> return! RequestErrors.BAD_REQUEST errors next ctx

        with exn ->
            let logger = ctx.GetLogger "Contacts"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx
    }