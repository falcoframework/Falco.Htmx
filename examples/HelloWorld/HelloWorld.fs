open Falco
open Falco.Markup
open Falco.Routing
open Falco.Htmx
open Microsoft.AspNetCore.Builder

let handleIndex : HttpHandler =
    let html =
        _html [] [
            _head [] [
                _script [ _src_ HtmxScript.cdnSrc ] [] ]
            _body [] [
                _h1' "Example: Hello World"
                _button
                    [ Hx.get "/click"
                      Hx.swapOuterHtml ]
                    [ _text "Click Me" ] ] ]

    Response.ofHtml html

let handleClick : HttpHandler =
    let html =
        _h2' "Hello, World from the Server!"

    Response.ofHtml html

let wapp = WebApplication.Create()

let endpoints =
    [
        get "/" handleIndex
        get "/click" handleClick
    ]

wapp.UseRouting()
    .UseFalco(endpoints)
    .Run()
