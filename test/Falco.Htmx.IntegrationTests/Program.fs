namespace Falco.Htmx.IntegrationTests

open System.Net.Http
open System.Text
open System.Text.Json
open Microsoft.AspNetCore.Mvc.Testing
open Xunit
open Falco.Htmx.IntegrationTests.App

module FalcoOpenApiTestServer =
    let createFactory() =
        new WebApplicationFactory<Program>()

module Tests =
    let private factory = FalcoOpenApiTestServer.createFactory ()

module Program = let [<EntryPoint>] main _ = 0
