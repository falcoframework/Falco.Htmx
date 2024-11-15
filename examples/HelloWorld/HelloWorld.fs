open Falco
open Falco.Markup
open Falco.Routing
open Falco.Htmx
open Microsoft.AspNetCore.Builder

let handleIndex : HttpHandler =
    let html =
        Elem.html [] [
            Elem.head [] [
                Elem.script [ Attr.src HtmxScript.cdnSrc ] [] ]
            Elem.body [] [
                Text.h1 "Example: Hello World"
                Elem.button
                    [ Hx.get "/click"
                      Hx.swapOuterHtml ]
                    [ Text.raw "Click Me" ] ] ]

    Response.ofHtml html

let handleClick : HttpHandler =
    let html =
        Text.h2 "Hello, World from the Server!"

    Response.ofHtml html

let wapp = WebApplication.Create()

let endpoints =
    [
        get "/" handleIndex
        get "/click" handleClick
    ]

wapp.UseFalco(endpoints)
    .Run()
