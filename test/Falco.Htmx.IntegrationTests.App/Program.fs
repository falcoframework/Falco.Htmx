module Falco.Htmx.IntegrationTests.App

open Falco
open Falco.Markup
open Falco.Routing
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

let endpoints =
    [
    ]

let bldr = WebApplication.CreateBuilder()

let wapp = bldr.Build()

wapp.UseHttpsRedirection()
|> ignore

wapp.UseFalco(endpoints)
|> ignore

wapp.Run()

type Program() = class end
