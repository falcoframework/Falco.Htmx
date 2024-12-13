open System
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
                Elem.table [] [
                    Elem.thead [] [
                        Elem.th [] [ Text.raw "Name" ]
                        Elem.th [] [ Text.raw "Email" ]
                        Elem.th [] [ Text.raw "ID" ]
                    ]
                    Elem.tbody [] [
                        for i = 10 to 20 do
                            Elem.tr [] [
                                Elem.td [] [ Text.raw "Agent Smith" ]
                                Elem.td [] [ Text.raw $"void{i}@null.org" ]
                                Elem.td [] [ Text.raw (Guid.NewGuid().ToString("n")) ]
                            ]

                        Elem.tr [] [
                            Elem.td [ Attr.colspan "3" ] [
                                Elem.button [ Hx.get "/"; Hx.tag] [
                                    Text.raw "Load More Agents..." ] ]
                        ]
                    ]
                ]
            ]
        ]

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

wapp.UseRouting()
    .UseFalco(endpoints)
    .Run()
