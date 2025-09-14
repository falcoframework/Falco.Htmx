open System
open Falco
open Falco.Markup
open Falco.Routing
open Falco.Htmx
open Microsoft.AspNetCore.Builder

let view (page : int) =
    let loadMoreButton (page : int) =
        _tr [ _id_ "replace-me" ] [
            _td [ _colspan_ "3" ] [
                _button [
                    Hx.get $"/contacts?page={page}"
                    Hx.targetCss "#replace-me"
                    Hx.swapOuterHtml
                    Hx.select "tr"
                ] [ _text "Load More Agents..." ] ]
        ]

    let agentRows (page : int) =
        [
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
                _tbody [ _id_ "agent-rows" ] (agentRows page)
            ]
        ]
    ]

let handleIndex : HttpHandler =
    Response.ofHtml (view 1)

let handleClick : HttpHandler =
    Request.mapQuery
        (fun q -> q.GetInt "page")
        (fun page ->
            Response.ofFragment "agent-rows" (view page))

let wapp = WebApplication.Create()

let endpoints =
    [
        get "/" handleIndex
        get "/contacts" handleClick
    ]

wapp.UseRouting()
    .UseFalco(endpoints)
    .Run()
