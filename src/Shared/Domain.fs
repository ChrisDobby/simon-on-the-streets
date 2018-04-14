namespace Domain

open System

type Contact = 
    {
        Id: int
        Name: string
        Registered: DateTime
    }

type ContactRequest = 
    {
        Name: string
    }