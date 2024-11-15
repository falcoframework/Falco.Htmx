namespace Falco.Htmx.Tests

open Falco.Htmx
open Falco.Markup
open FsUnit.Xunit
open Xunit

[<AutoOpen>]
module private Common =
    let testElem attr =
        Elem.div attr [ Text.raw "div" ]
        |> renderNode

module HxTests =
    [<Fact>]
    let ``Hx.get should produce element with hx-get attribute`` () =
        testElem [ Hx.get "/" ]
        |> should equal """<div hx-get="/">div</div>"""

    [<Fact>]
    let ``Hx.post should produce element with hx-post attribute`` () =
        testElem [ Hx.post "/" ]
        |> should equal """<div hx-post="/">div</div>"""

    [<Fact>]
    let ``Hx.put should produce element with hx-put attribute`` () =
        testElem [ Hx.put "/" ]
        |> should equal """<div hx-put="/">div</div>"""

    [<Fact>]
    let ``Hx.patch should produce element with hx-patch attribute`` () =
        testElem [ Hx.patch "/" ]
        |> should equal """<div hx-patch="/">div</div>"""

    [<Fact>]
    let ``Hx.delete should produce element with hx-delete attribute`` () =
        testElem [ Hx.delete "/" ]
        |> should equal """<div hx-delete="/">div</div>"""

    [<Fact>]
    let ``Hx.trigger should produce element with hx-trigger attribute`` () =
        [
            testElem [ Hx.trigger "click" ], "click"
            testElem [ Hx.trigger ("click", filter = "ctrlKey") ], "click[ctrlKey]"
            testElem [ Hx.trigger ("click", [ HxTrigger.Once ], "ctrlKey") ], "click[ctrlKey] once"

            testElem [ Hx.trigger ("click", [ HxTrigger.Once ]) ], "click once"
            testElem [ Hx.trigger ("click", [ HxTrigger.Changed ]) ], "click changed"
            testElem [ Hx.trigger ("click", [ HxTrigger.Delay (HxTime.Ms 1)]) ], "click delay:1ms"
            testElem [ Hx.trigger ("click", [ HxTrigger.Delay (HxTime.Sec 1)]) ], "click delay:1s"
            testElem [ Hx.trigger ("click", [ HxTrigger.Delay (HxTime.Min 1)]) ], "click delay:1m"
            testElem [ Hx.trigger ("click", [ HxTrigger.Throttle (HxTime.Ms 1)]) ], "click throttle:1ms"
            testElem [ Hx.trigger ("click", [ HxTrigger.Throttle (HxTime.Sec 1)]) ], "click throttle:1s"
            testElem [ Hx.trigger ("click", [ HxTrigger.Throttle (HxTime.Min 1)]) ], "click throttle:1m"
            testElem [ Hx.trigger ("click", [ HxTrigger.FromWindow ]) ], "click from:window"
            testElem [ Hx.trigger ("click", [ HxTrigger.FromDocument ]) ], "click from:document"
            testElem [ Hx.trigger ("click", [ HxTrigger.From HxTarget.This ]) ], "click from:this"
            testElem [ Hx.trigger ("click", [ HxTrigger.From HxTarget.NextSibling ]) ], "click from:next"
            testElem [ Hx.trigger ("click", [ HxTrigger.From HxTarget.PreviousSibling ]) ], "click from:previous"
            testElem [ Hx.trigger ("click", [ HxTrigger.From (HxTarget.Css "#falco") ]) ], "click from:#falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.From (HxTarget.Closest ".falco") ]) ], "click from:closest .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.From (HxTarget.Find ".falco") ]) ], "click from:find .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.From (HxTarget.Next ".falco") ]) ], "click from:next .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.From (HxTarget.Previous ".falco") ]) ], "click from:previous .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target HxTarget.This ]) ], "click target:this"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target HxTarget.NextSibling ]) ], "click target:next"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target HxTarget.PreviousSibling ]) ], "click target:previous"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target (HxTarget.Css "#falco") ]) ], "click target:#falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target (HxTarget.Closest ".falco") ]) ], "click target:closest .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target (HxTarget.Find ".falco") ]) ], "click target:find .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target (HxTarget.Next ".falco") ]) ], "click target:next .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.Target (HxTarget.Previous ".falco") ]) ], "click target:previous .falco"
            testElem [ Hx.trigger ("click", [ HxTrigger.Consume ]) ], "click consume"
            testElem [ Hx.trigger ("click", [ HxTrigger.QueueFirst ]) ], "click queue:first"
            testElem [ Hx.trigger ("click", [ HxTrigger.QueueLast ]) ], "click queue:last"
            testElem [ Hx.trigger ("click", [ HxTrigger.QueueAll ]) ], "click queue:all"
            testElem [ Hx.trigger ("click", [ HxTrigger.QueueNone ]) ], "click queue:none"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-trigger="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.target should produce element with hx-target attribute`` () =
        [
            testElem [ Hx.targetThis ], "this"
            testElem [ Hx.targetNextSibling ], "next"
            testElem [ Hx.targetPreviousSibling ], "previous"
            testElem [ Hx.targetCss "#falco" ], "#falco"
            testElem [ Hx.targetClosest ".falco" ], "closest .falco"
            testElem [ Hx.targetFind ".falco" ], "find .falco"
            testElem [ Hx.targetNext ".falco" ], "next .falco"
            testElem [ Hx.targetPrevious ".falco" ], "previous .falco"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-target="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.swap should produce element with hx-swap attribute`` () =
        [
            testElem [ Hx.swapInnerHtml ], "innerHTML"
            testElem [ Hx.swapOuterHtml ], "outerHTML"
            testElem [ Hx.swapBeforeBegin ], "beforebegin"
            testElem [ Hx.swapAfterBegin ], "afterbegin"
            testElem [ Hx.swapBeforeEnd ], "beforeend"
            testElem [ Hx.swapAfterEnd ], "afterend"
            testElem [ Hx.swapDelete ], "delete"
            testElem [ Hx.swapNone ], "none"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-swap="{attrValue}">div</div>""")


    [<Fact>]
    let ``Hx.swapOob should produce element with hx-swap-oob attribute`` () =
        [
            testElem [ Hx.swapOobOn ], "outerHTML"
            testElem [ Hx.swapOob HxSwap.InnerHTML ], "innerHTML"
            testElem [ Hx.swapOob HxSwap.OuterHTML ], "outerHTML"
            testElem [ Hx.swapOob HxSwap.BeforeBegin ], "beforebegin"
            testElem [ Hx.swapOob HxSwap.AfterBegin ], "afterbegin"
            testElem [ Hx.swapOob HxSwap.BeforeEnd ], "beforeend"
            testElem [ Hx.swapOob HxSwap.AfterEnd ], "afterend"
            testElem [ Hx.swapOob HxSwap.Delete ], "delete"
            testElem [ Hx.swapOob HxSwap.NoSwap ], "none"
            testElem [ Hx.swapOob (HxSwap.InnerHTML, "#falco") ], "innerHTML:#falco"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-swap-oob="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.select should produce element with hx-select attribute `` () =
        [
            testElem [ Hx.select "#falco" ], "#falco"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-select="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.selectOob should produce element with hx-select-oob attribute `` () =
        [
            testElem [ Hx.selectOob "#falco" ], "#falco"
            testElem [ Hx.selectOob ["#falco"] ], "#falco"
            testElem [ Hx.selectOob [ "#falco"; "#htmx" ] ] , "#falco,#htmx"
            testElem [ Hx.selectOob [ "#falco", HxSwap.OuterHTML ] ] , "#falco:outerHTML"
            testElem [ Hx.selectOob [ "#falco", HxSwap.OuterHTML; "#htmx", HxSwap.InnerHTML ] ] , "#falco:outerHTML,#htmx:innerHTML"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-select-oob="{attrValue}">div</div>""")

    [<Theory>]
    [<InlineData(true, "true")>]
    [<InlineData(false, "false")>]
    let ``Hx.boost should produce element with hx-boost attribute`` (enabled, attrValue) =
        testElem [ Hx.boost enabled ]
        |> should equal $"""<div hx-boost="{attrValue}">div</div>"""

    [<Fact>]
    let ``Hx.pushUrl should produce element with hx-push-url attribute`` () =
        [
            testElem [ Hx.pushUrl true ], "true"
            testElem [ Hx.pushUrlOn ], "true"
            testElem [ Hx.pushUrl false ], "false"
            testElem [ Hx.pushUrlOff ], "false"
            testElem [ Hx.pushUrl "/" ], "/"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-push-url="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.sync should produce element with hx-sync attribute`` () =
        [
            testElem [ Hx.sync "this" ], "this"
            testElem [ Hx.sync "#falco" ], "#falco"
            testElem [ Hx.sync (".falco", HxSync.Drop) ], ".falco:drop"
            testElem [ Hx.sync (".falco", HxSync.Abort) ], ".falco:abort"
            testElem [ Hx.sync (".falco", HxSync.Replace) ], ".falco:replace"
            testElem [ Hx.sync (".falco", HxSync.QueueFirst) ], ".falco:queue first"
            testElem [ Hx.sync (".falco", HxSync.QueueLast) ], ".falco:queue last"
            testElem [ Hx.sync (".falco", HxSync.QueueAll) ], ".falco:queue all"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-sync="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.include' should produce element with hx-include attribute`` () =
        [
            testElem [ Hx.include' (HxTarget.This) ], "this"
            testElem [ Hx.includeThis ], "this"
            testElem [ Hx.includeNextSibling ], "next"
            testElem [ Hx.includePreviousSibling ], "previous"
            testElem [ Hx.includeCss "#falco" ], "#falco"
            testElem [ Hx.includeClosest ".falco" ], "closest .falco"
            testElem [ Hx.includeFind ".falco" ], "find .falco"
            testElem [ Hx.includeNext ".falco" ], "next .falco"
            testElem [ Hx.includePrevious ".falco" ], "previous .falco"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-include="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.params' should produce element with hx-params attribute`` () =
        [
            testElem [ Hx.paramsAll ], "*"
            testElem [ Hx.paramsNone ], "none"
            testElem [ Hx.paramsExclude [ "name" ] ], "not name"
            testElem [ Hx.paramsExclude [ "name"; "email" ] ],  "not name, email"
            testElem [ Hx.paramsInclude [ "name" ] ], "name"
            testElem [ Hx.paramsInclude [ "name"; "email" ] ], "name, email"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-params="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.vals' should produce element with hx-vals attribute`` () =
        [
            testElem [ Hx.vals """{ "name": "falco" }""" ], """{ "name": "falco" }"""
            testElem [ Hx.vals "js:{ name: 'falco' }" ], "js:{ name: 'falco' }"
            testElem [ Hx.valsJs "{ name: 'falco' }" ], "js:{ name: 'falco' }"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-vals="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.confirm should produce element with hx-confirm attribute`` () =
        [
            testElem [ Hx.confirm "Confirm message" ], "Confirm message"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-confirm="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.disable should produce element with hx-disable attribute`` () =
        testElem [ Hx.disable ]
        |> should equal """<div hx-disable>div</div>"""


    [<Fact>]
    let ``Hx.disabled should produce element with hx-disabled-elt attribute`` () =
        [
            testElem [ Hx.disabled (HxTarget.This) ], "this"
            testElem [ Hx.disabledThis ], "this"
            testElem [ Hx.disabledNextSibling ], "next"
            testElem [ Hx.disabledPreviousSibling ], "previous"
            testElem [ Hx.disabledCss "#falco" ], "#falco"
            testElem [ Hx.disabledClosest ".falco" ], "closest .falco"
            testElem [ Hx.disabledFind ".falco" ], "find .falco"
            testElem [ Hx.disabledNext ".falco" ], "next .falco"
            testElem [ Hx.disabledPrevious ".falco" ], "previous .falco"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-disabled-elt="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.inherit' should produce element with hx-inherit attribute`` () =
        [
            testElem [ Hx.inheritAll ], "*"
            testElem [ Hx.inherit' "*" ], "*"
            testElem [ Hx.inherit' "hx-get" ], "hx-get"
            testElem [ Hx.inherit' "hx-swap hx-select" ], "hx-swap hx-select"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-inherit="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.disinherit should produce element with hx-disinherit attribute`` () =
        [
            testElem [ Hx.disinheritAll ], "*"
            testElem [ Hx.disinherit "*" ], "*"
            testElem [ Hx.disinherit "hx-get" ], "hx-get"
            testElem [ Hx.disinherit "hx-swap hx-select" ], "hx-swap hx-select"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-disinherit="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.ext should produce element with hx-ext attribute`` () =
        [
            testElem [ Hx.ext "example" ], "example"
            testElem [ Hx.ext "ignore:example" ], "ignore:example"
            testElem [ Hx.extIgnore "example" ], "ignore:example"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-ext="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.headers should produce element with hx-headers attribute`` () =
        [
            testElem [ Hx.headers [] ], "{}"
            testElem [ Hx.headers ([], false) ], "{}"
            testElem [ Hx.headers ([], true) ], "js:{}"
            testElem [ Hx.headers [ "h", "v" ] ], """{"h":"v"}"""
            testElem [ Hx.headers ([ "h", "v" ], false) ], """{"h":"v"}"""
            testElem [ Hx.headers ([ "h", "v" ], true) ], """js:{"h":"v"}"""
            testElem [ Hx.headers [ "h", "v"; "h2", "v2" ] ], """{"h":"v","h2":"v2"}"""
            testElem [ Hx.headers ([ "h", "v"; "h2", "v2" ], false) ], """{"h":"v","h2":"v2"}"""
            testElem [ Hx.headers ([ "h", "v"; "h2", "v2" ], true) ], """js:{"h":"v","h2":"v2"}"""
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal ($"""<div hx-headers="{attrValue}">div</div>"""))

    [<Fact>]
    let ``Hx.historyOff should produce element with hx-history attribute`` () =
        testElem [ Hx.historyOff ]
        |> should equal """<div hx-history="false">div</div>"""

    [<Fact>]
    let ``Hx.historyElt should produce element with hx-history-elt attribute`` () =
        testElem [ Hx.historyElt ]
        |> should equal "<div hx-history-elt>div</div>"

    [<Fact>]
    let ``Hx.indicator should produce element with hx-indicator attribute`` () =
        [
            testElem [ Hx.indicator "#falco" ], "#falco"
            testElem [ Hx.indicator "#falco, #falco2" ], "#falco, #falco2"
            testElem [ Hx.indicatorClosest "tr" ], "closest tr"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-indicator="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.preserve should produce element with hx-preserve attribute`` () =
        testElem [ Hx.preserve ]
        |> should equal "<div hx-preserve>div</div>"

    [<Fact>]
    let ``Hx.prompt should produce element with hx-prompt attribute`` () =
        testElem [ Hx.prompt "I am a prompt" ]
        |> should equal """<div hx-prompt="I am a prompt">div</div>"""

    [<Fact>]
    let ``Hx.replaceUrl should produce element with hx-replace-url attribute`` () =
        [
            testElem [ Hx.replaceUrl true ], "true"
            testElem [ Hx.replaceUrl false ], "false"
            testElem [ Hx.replaceUrlOn ], "true"
            testElem [ Hx.replaceUrlOff ], "false"
            testElem [ Hx.replaceUrl "/" ], "/"
            testElem [ Hx.replaceUrl "/hello-world" ], "/hello-world"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal $"""<div hx-replace-url="{attrValue}">div</div>""")

    [<Fact>]
    let ``Hx.validate should produce element with hx-validate attribute`` () =
        testElem [ Hx.validate ]
        |> should equal """<div hx-validate="true">div</div>"""

    [<Fact>]
    let ``Hx.request should produce element with hx-request attribute`` () =
        [
            testElem [ Hx.request () ], "{}"
            testElem [ Hx.request (evaluate = true) ], "js:{}"
            testElem [ Hx.request (timeout = 1) ], """{"timeout":1}"""
            testElem [ Hx.request (timeout = 1, evaluate = true) ], """js:{"timeout":1}"""
            testElem [ Hx.request (credentials = true, noHeaders = true) ], """{"credentials":true,"noHeaders":true}"""
            testElem [ Hx.request (credentials = true, noHeaders = true, evaluate = true) ], """js:{"credentials":true,"noHeaders":true}"""
            testElem [ Hx.request (timeout = 1, credentials = true) ], """{"timeout":1,"credentials":true}"""
            testElem [ Hx.request (timeout = 1, credentials = true, evaluate = true) ], """js:{"timeout":1,"credentials":true}"""
            testElem [ Hx.request (timeout = 1, noHeaders = true) ], """{"timeout":1,"noHeaders":true}"""
            testElem [ Hx.request (timeout = 1, noHeaders = true, evaluate = true) ], """js:{"timeout":1,"noHeaders":true}"""
            testElem [ Hx.request (timeout = 1, credentials = true, noHeaders = true) ], """{"timeout":1,"credentials":true,"noHeaders":true}"""
            testElem [ Hx.request (timeout = 1, credentials = true, noHeaders = true, evaluate = true) ], """js:{"timeout":1,"credentials":true,"noHeaders":true}"""
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> should equal ("<div hx-request=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.encodingMultipart should produce element with hx-encoding attribute`` () =
        testElem [ Hx.encodingMultipart ]
        |> should equal """<div hx-encoding="multipart/form-data">div</div>"""
