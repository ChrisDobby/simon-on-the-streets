module Materialize

open Fable.Core
open Fable.Import

[<Emit("JSON.parse($0)")>]
let jsonParse (json: string) : obj option = jsNative

[<Emit("window['$'](document).ready(function() { var elem = document.querySelector($0);M.Autocomplete.init(elem, {data: $1}); })")>]
let initialiseAutocomplete (selector: string) (data: obj) : unit = jsNative

let initialiseAutocompleteFromJsonString (selector: string) (json: string) : unit = initialiseAutocomplete selector (jsonParse(json))

let autoCompleteJsonFromList options =
    let optionJson option = sprintf "\"%s\":null" option
    options |>
    List.fold(fun acc option -> 
        match acc with 
            | "" -> optionJson option
            | _ -> sprintf "%s,%s" acc (optionJson option)) ""

let initialiseAutocompleteFromList (selector: string) (options: string list) : unit =
    let jsonString = sprintf """{%s}"""(autoCompleteJsonFromList options)
    initialiseAutocompleteFromJsonString selector jsonString

[<Emit("window['$'](document).ready(function() { M.AutoInit(); })")>]
let autoInit () : unit = jsNative

[<Emit("window['$'](document).ready(function() { var elem = document.querySelector($0);M.Modal.init(elem); })")>]
let initialiseModal (selector: string) = jsNative