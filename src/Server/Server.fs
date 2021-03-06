﻿open System
open System.IO

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Newtonsoft.Json

open Giraffe
open Giraffe.Serialization.Json

let inMemoryDataFunctions =
    (fun () -> task { return Db.InMemory.Data.getContacts () }),
    (fun id -> task { return Db.InMemory.Data.getContact id }),
    (fun reg -> task { return Db.InMemory.Data.addContact reg }),
    (fun () -> task { return Db.InMemory.Data.getPreviousContacts () })


let clientPath =
    // did we start from server folder?
    let devPath = Path.Combine("..","Client")
    if Directory.Exists devPath then devPath
    else
        // maybe we are in root of project?
        let devPath = Path.Combine("src","Client")
        if Directory.Exists devPath then devPath
        else @"./client"
    |> Path.GetFullPath

let port = 8085us

let configureApp  (app : IApplicationBuilder) =
  app.UseStaticFiles()
     .UseGiraffe(Web.App clientPath inMemoryDataFunctions)

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

    let fableJsonSettings = JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())

    services.AddSingleton<IJsonSerializer>(
        NewtonsoftJsonSerializer(fableJsonSettings)) |> ignore

WebHost
  .CreateDefaultBuilder()
  .UseWebRoot(clientPath)
  .UseContentRoot(clientPath)
  .Configure(Action<IApplicationBuilder> configureApp)
  .ConfigureServices(configureServices)
  .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
  .Build()
  .Run()