﻿open Falco
open Falco.Markup
open Falco.Routing
open Falco.Htmx
open Microsoft.AspNetCore.Builder

module View =
    let template content =
        _html [ _lang_ "en" ] [
            _head [] [
                _script [ _src_ HtmxScript.cdnSrc ] [] ]
            _body []
                content ]

    module Components =
        let clicker =
            _button
                [ Hx.get "/click"
                  Hx.swapOuterHtml ]
                [ _text "Click Me" ]

        let resetter =
            _div [ _id_ "wrapper" ] [
                _h2' "Way to go! You clicked it!"
                _br []
                _button
                    [ Hx.get "/reset"
                      Hx.swapOuterHtml
                      Hx.targetCss "#wrapper" ]
                    [ _text "Reset" ] ]

module App =
    let handleIndex : HttpHandler =
        let html =
            View.template [
                _h1' "Example: Click & Swap"
                View.Components.clicker ]

        Response.ofHtml html

    let handleClick : HttpHandler =
        Response.ofHtml View.Components.resetter

    let handleReset : HttpHandler =
        Response.ofHtml View.Components.clicker


let wapp = WebApplication.Create()

let endpoints =
    [
        get "/" App.handleIndex
        get "/click" App.handleClick
        get "/reset" App.handleReset
    ]

wapp.UseRouting()
    .UseFalco(endpoints)
    .Run()
