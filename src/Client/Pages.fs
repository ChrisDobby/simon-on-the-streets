module Pages

open Elmish.Browser.UrlParser

type Page = 
    | Home
    | AddContact

let toPath = function
    | Page.Home -> "/"
    | Page.AddContact -> "/add"


/// The URL is turned into a Result.
let pageParser : Parser<(Page -> Page),_> =
    oneOf
        [ 
            map Home (s "") 
            map AddContact (s "add")
        ]

let urlParser location = parsePath pageParser location