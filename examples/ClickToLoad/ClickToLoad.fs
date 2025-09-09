open System
open Falco
open Falco.Markup
open Falco.Routing
open Falco.Htmx
open Microsoft.AspNetCore.Builder

let loadMoreButton (page : int) =
    _tr [ _id_ "replaceMe" ] [
        _td [ _colspan_ "3" ] [
            _button [
                Hx.get $"/contacts?page={page}"
                Hx.targetCss "#replaceMe"
                Hx.swapOuterHtml
            ] [ _text "Load More Agents..." ] ]
    ]

let agentRows (page : int) =
    Elem.createFragment [
        let min = (page - 1) * 10 + 1
        let max = page * 10
        for i = min to max do
            _tr [] [
                _td [] [ _text "Agent Smith" ]
                _td [] [ _text $"void{i}@null.org" ]
                _td [] [ _text (Guid.NewGuid().ToString "n") ]
            ]
        loadMoreButton (page + 1)
    ]

let handleIndex : HttpHandler =
    let html =
        _html [] [
            _head [] [
                _script [ _src_ HtmxScript.cdnSrc ] [] ]
            _body [] [
                _table [] [
                    _thead [] [
                        _th [] [ _text "Name" ]
                        _th [] [ _text "Email" ]
                        _th [] [ _text "ID" ]
                    ]
                    _tbody [] [
                        agentRows 1
                    ]
                ]
            ]
        ]

    Response.ofHtml html

let handleClick : HttpHandler =
    Request.mapQuery (fun q -> q.GetInt "page")
        (agentRows >> Response.ofHtml)

let wapp = WebApplication.Create()

let endpoints =
    [
        get "/" handleIndex
        get "/contacts" handleClick
    ]

wapp.UseRouting()
    .UseFalco(endpoints)
    .Run()
