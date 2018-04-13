module Registrations

open Giraffe
open ServerErrors
open Domain
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open System.Threading.Tasks

let getAllRegistrations (getRegistrationsFromDB: unit -> Task<Registration list>) next (ctx: HttpContext) =
    task {
        try
            let! registrations = getRegistrationsFromDB ()
            return! Successful.OK (registrations) next ctx
        with exn ->
            let logger = ctx.GetLogger "Registrations"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx
    }

let getRegistration (getRegistrationFromDb: int -> Task<Option<Registration>>) id next (ctx: HttpContext) = 
    task {
        try
            let! registration = getRegistrationFromDb(id)

            match registration with
                | Some(reg) -> return! Successful.OK (reg) next ctx
                | _ -> return! RequestErrors.NOT_FOUND "registration not found" next ctx

        with exn ->
            let logger = ctx.GetLogger "Registrations"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx        
    }

let register (addRegistration: RegistrationRequest -> Task<Registration>) next (ctx: HttpContext) =
    task {
        try
            let! registerRequest = ctx.BindJsonAsync<RegistrationRequest>()

            let! newRegistration = addRegistration registerRequest
            return! Successful.OK(newRegistration) next ctx
        with exn ->
            let logger = ctx.GetLogger "TeamCaptainTeams"
            logger.LogError (EventId(), exn, "SERVICE_UNAVAILABLE")
            return! SERVICE_UNAVAILABLE "Database not available" next ctx
    }