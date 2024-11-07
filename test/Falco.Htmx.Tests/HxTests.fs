namespace Falco.Htmx.Tests

open Falco.Htmx
open Falco.Markup
open FsUnit.Xunit
open Xunit

[<AutoOpen>]
module Common =
    let elem attr = Elem.div attr [ Text.raw "div" ]

module HxTests =
    [<Fact>]
    let ``Hx.get should produce element with hx-get attribute`` () =
        elem [ Hx.get "/" ] |> renderNode |> should equal "<div hx-get=\"/\">div</div>"

    [<Fact>]
    let ``Hx.post should produce element with hx-post attribute`` () =
        elem [ Hx.post "/" ]
        |> renderNode
        |> should equal "<div hx-post=\"/\">div</div>"

    [<Fact>]
    let ``Hx.put should produce element with hx-put attribute`` () =
        elem [ Hx.put "/" ] |> renderNode |> should equal "<div hx-put=\"/\">div</div>"

    [<Fact>]
    let ``Hx.patch should produce element with hx-patch attribute`` () =
        elem [ Hx.patch "/" ]
        |> renderNode
        |> should equal "<div hx-patch=\"/\">div</div>"

    [<Fact>]
    let ``Hx.delete should produce element with hx-delete attribute`` () =
        elem [ Hx.delete "/" ]
        |> renderNode
        |> should equal "<div hx-delete=\"/\">div</div>"

    [<Theory>]
    [<InlineData(true, "true")>]
    [<InlineData(false, "false")>]
    let ``Hx.boost should produce element with hx-boost attribute`` (enabled, attrValue) =
        elem [ Hx.boost enabled ]
        |> renderNode
        |> should equal ("<div hx-boost=\"" + attrValue + "\">div</div>")

    [<Fact>]
    let ``Hx.pushUrl should produce element with hx-push-url attribute`` () =
        [
            elem [ Hx.pushUrl Hx.Url.true' ], "true"
            elem [ Hx.pushUrl Hx.Url.false' ], "false"
            elem [ Hx.pushUrl (Hx.Url.path "/") ], "/"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-push-url=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.select should produce element with hx-select attribute`` () =
        [
            elem [ Hx.select Hx.Target.this ], "this"
            elem [ Hx.select (Hx.Target.css "#info-details") ], "#info-details"
            elem [ Hx.select (Hx.Target.closest "tr") ], "closest tr"
            elem [ Hx.select (Hx.Target.find "table") ], "find table"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-select=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.selectOob should produce element with hx-select-oob attribute`` () =
        [
            elem [ Hx.selectOob Hx.Target.this ], "this"
            elem [ Hx.selectOob (Hx.Target.css "#info-details") ], "#info-details"
            elem [ Hx.selectOob (Hx.Target.closest "tr") ], "closest tr"
            elem [ Hx.selectOob (Hx.Target.find "table") ], "find table"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-select-oob=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.swap should produce element with hx-swap attribute`` () =
        [
            elem [ Hx.swap Hx.Swap.innerHTML ], "innerHTML"
            elem [ Hx.swap Hx.Swap.outerHTML ], "outerHTML"
            elem [ Hx.swap Hx.Swap.beforebegin ], "beforebegin"
            elem [ Hx.swap Hx.Swap.afterbegin ], "afterbegin"
            elem [ Hx.swap Hx.Swap.beforeend ], "beforeend"
            elem [ Hx.swap Hx.Swap.afterend ], "afterend"
            elem [ Hx.swap Hx.Swap.delete ], "delete"
            elem [ Hx.swap Hx.Swap.none ], "none"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-swap=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.swapOob should produce element with hx-swap-oob attribute`` () =
        [
            elem [ Hx.swapOob Hx.SwapOob.true' ], "true"
            elem [ Hx.swapOob Hx.SwapOob.innerHTML ], "innerHTML"
            elem [ Hx.swapOob Hx.SwapOob.outerHTML ], "outerHTML"
            elem [ Hx.swapOob Hx.SwapOob.beforebegin ], "beforebegin"
            elem [ Hx.swapOob Hx.SwapOob.afterbegin ], "afterbegin"
            elem [ Hx.swapOob Hx.SwapOob.beforeend ], "beforeend"
            elem [ Hx.swapOob Hx.SwapOob.afterend ], "afterend"
            elem [ Hx.swapOob Hx.SwapOob.delete ], "delete"
            elem [ Hx.swapOob Hx.SwapOob.none ], "none"
            elem [ Hx.swapOob (Hx.SwapOob.innerHTMLTarget (Hx.Target.css "#info-details")) ], "innerHTML:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.outerHTMLTarget (Hx.Target.css "#info-details")) ], "outerHTML:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.beforebeginTarget (Hx.Target.css "#info-details")) ],
            "beforebegin:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.afterbeginTarget (Hx.Target.css "#info-details")) ],
            "afterbegin:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.beforeendTarget (Hx.Target.css "#info-details")) ], "beforeend:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.afterendTarget (Hx.Target.css "#info-details")) ], "afterend:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.deleteTarget (Hx.Target.css "#info-details")) ], "delete:#info-details"
            elem [ Hx.swapOob (Hx.SwapOob.noneTarget (Hx.Target.css "#info-details")) ], "none:#info-details"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-swap-oob=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.target should produce element with hx-target attribute`` () =
        [
            elem [ Hx.target Hx.Target.this ], "this"
            elem [ Hx.target (Hx.Target.css "#info-details") ], "#info-details"
            elem [ Hx.target (Hx.Target.closest "tr") ], "closest tr"
            elem [ Hx.target (Hx.Target.find "table") ], "find table"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-target=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.trigger should produce element with hx-trigger attribute`` () =
        []
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-trigger=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.vals should produce element with hx-vals attribute`` () =
        [ elem [ Hx.vals {| myVal = "My Value" |} ], "{\"myVal\":\"My Value\"}" ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-vals=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.valsJs should produce element with hx-vals attribute`` () =
        [ elem [ Hx.valsJs "{myVal: calculateValue()}" ], "js:{myVal: calculateValue()}" ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-vals=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.confirm should produce element with hx-confirm attribute`` () =
        elem [ Hx.confirm "Confirm message" ]
        |> renderNode
        |> should equal "<div hx-confirm=\"Confirm message\">div</div>"

    [<Fact>]
    let ``Hx.disable should produce element with hx-disable attribute`` () =
        elem [ Hx.disable ] |> renderNode |> should equal "<div hx-disable>div</div>"

    [<Fact>]
    let ``Hx.disinherit should produce element with hx-disinherit attribute`` () =
        [
            elem [ Hx.disinherit Hx.Disinherit.all ], "*"
            elem [ Hx.disinherit (Hx.Disinherit.exclude []) ], "*"
            elem [ Hx.disinherit (Hx.Disinherit.exclude [ "hx-select" ]) ], "hx-select"
            elem [
                Hx.disinherit (
                    Hx.Disinherit.exclude [
                        "hx-select"
                        "hx-get"
                    ]
                )
            ],
            "hx-select hx-get"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-disinherit=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.encoding should produce element with hx-encoding attribute`` () =
        elem [ Hx.encoding "multipart/form-data" ]
        |> renderNode
        |> should equal "<div hx-encoding=\"multipart/form-data\">div</div>"

    [<Fact>]
    let ``Hx.ext should produce element with hx-ext attribute`` () =
        [
            elem [ Hx.ext "example" ], "example"
            elem [ Hx.ext "ignore:example" ], "ignore:example"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-ext=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.headers should produce element with hx-headers attribute`` () =
        [
            elem [ Hx.headers [] ], "{}"
            elem [ Hx.headers [ "myHeader", "my value" ] ], "{\"myHeader\":\"my value\"}"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-headers=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.historyElt should produce element with hx-historyElt attribute`` () =
        elem [ Hx.historyElt ]
        |> renderNode
        |> should equal "<div hx-history-elt>div</div>"

    [<Fact>]
    let ``Hx.include' should produce element with hx-include attribute`` () =
        [
            elem [ Hx.include' [ Hx.Target.this ] ], "this"
            elem [ Hx.include' [ Hx.Target.css "#info-details" ] ], "#info-details"
            elem [ Hx.include' [ Hx.Target.closest "tr" ] ], "closest tr"
            elem [ Hx.include' [ Hx.Target.find "table" ] ], "find table"
            elem [
                Hx.include' [
                    Hx.Target.css "*"
                    Hx.Target.find "table"
                ]
            ],
            "*, find table"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-include=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.indicator should produce element with hx-indicator attribute`` () =
        [
            elem [ Hx.indicator Hx.Target.this ], "this"
            elem [ Hx.indicator (Hx.Target.css "#info-details") ], "#info-details"
            elem [ Hx.indicator (Hx.Target.closest "tr") ], "closest tr"
            elem [ Hx.indicator (Hx.Target.find "table") ], "find table"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-indicator=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.params' should produce element with hx-params attribute`` () =
        [
            elem [ Hx.params' Hx.Param.all ], "*"
            elem [ Hx.params' Hx.Param.none ], "none"
            elem [ Hx.params' (Hx.Param.exclude [ "name" ]) ], "not name"
            elem [
                Hx.params' (
                    Hx.Param.exclude [
                        "name"
                        "email"
                    ]
                )
            ],
            "not name, email"
            elem [ Hx.params' (Hx.Param.include' [ "name" ]) ], "name"
            elem [
                Hx.params' (
                    Hx.Param.include' [
                        "name"
                        "email"
                    ]
                )
            ],
            "name, email"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-params=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.preserve should produce element with hx-preserve attribute`` () =
        elem [ Hx.preserve ] |> renderNode |> should equal "<div hx-preserve>div</div>"

    [<Fact>]
    let ``Hx.prompt should produce element with hx-prompt attribute`` () =
        elem [ Hx.prompt "Prompt message" ]
        |> renderNode
        |> should equal "<div hx-prompt=\"Prompt message\">div</div>"

    [<Fact>]
    let ``Hx.replaceUrl should produce element with hx-replace-url attribute`` () =
        [
            elem [ Hx.replaceUrl Hx.Url.true' ], "true"
            elem [ Hx.replaceUrl Hx.Url.false' ], "false"
            elem [ Hx.replaceUrl (Hx.Url.path "/") ], "/"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-replace-url=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.request should produce element with hx-request attribute`` () =
        [
            elem [ Hx.request "\"timeout\":100" ], "\"timeout\":100"
            elem [ Hx.request "js: timeout:getTimeoutSetting()" ], "js: timeout:getTimeoutSetting()"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-request=\"" + attrValue + "\">div</div>"))

    [<Fact>]
    let ``Hx.sync should produce element with hx-sync attribute`` () =
        [
            elem [ Hx.sync (Hx.Target.this, None) ], "this"
            elem [ Hx.sync (Hx.Target.this, Some Hx.Sync.abort) ], "this:abort"
            elem [ Hx.sync (Hx.Target.closest "tr", Some Hx.Sync.replace) ], "closest tr:replace"
        ]
        |> List.iter (fun (elem, attrValue) ->
            elem
            |> renderNode
            |> should equal ("<div hx-sync=\"" + attrValue + "\">div</div>"))
