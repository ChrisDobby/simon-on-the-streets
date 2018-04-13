module Pages

open Elmish.Browser.UrlParser

type Page = 
    | Home

let toPath = function
    | Page.Home -> "/"


/// The URL is turned into a Result.
let pageParser : Parser<(Page -> Page),_> =
    oneOf
        [ 
            map Home (s "") 
        ]

let urlParser location = parsePath pageParser location