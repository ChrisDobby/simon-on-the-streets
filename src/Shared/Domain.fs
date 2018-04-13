namespace Domain

open System

type Registration = 
    {
        Id: int
        Name: string
        Registered: DateTime
    }

type RegistrationRequest = 
    {
        Name: string
    }