[<RequireQualifiedAccess>]
module Falco.Htmx.Hx

open System
open System.Text.Json
open Falco.Markup
open FSharp.Core

/// The hx-target attribute allows you to target a different element for
/// swapping than the one issuing the AJAX request.
module Target =
    /// Indicates that the element that the hx-target attribute is on is the target.
    let this = This

    /// Resolves to element.nextElementSibling.
    let nextSibling = NextSibling

    /// Resolves to element.previousElementSibling.
    let previousSibling = PreviousSibling

    /// A CSS query selector of the element to target.
    let css selector = CssSelector selector

    /// Find the closest ancestor element or itself, that matches the given CSS selector
    let closest selector = Closest selector

    /// Find the first child descendant element that matches the given CSS selector.
    let find selector = Find selector

    /// Scan the DOM forward for the first element that matches the given CSS selector.
    let next selector = Next selector

    /// Scan the DOM backward for the first element that matches the given CSS selector.
    let previous selector = Previous selector


module Timing =
    /// Millisecond-based timing declaration.
    let ms (milliseconds: float) = Milliseconds milliseconds

    /// Second-based timing declaration.
    let s (seconds: float) = Seconds seconds

    /// Minute-based timing declaration.
    let minutes (minutes: float) = Minutes minutes

module Trigger =
    /// Specify the discrete trigger event.
    let event (name: string, filters: string option, modifiers: EventModifier list) =
        Event(name, filters, modifiers)

    /// Specifiy the timing declaration for polling.
    let poll (timing: TimingDeclaration) = Poll timing

module Swap =
    /// The default, puts the content inside the target element.
    let innerHTML = InnerHTML

    /// Replaces the entire target element with the returned content.
    let outerHTML = OuterHTML

    /// Prepends the content before the first child inside the target.
    let beforebegin = BeforeBegin

    /// Prepends the content before the target in the target's parent element.
    let afterbegin = AfterBegin

    /// Appends the content after the last child inside the target.
    let beforeend = BeforEend

    /// Appends the content after the target in the target's parent element.
    let afterend = AfterEnd

    /// Deletes the target element regardless of the response.
    let delete = Delete

    /// Does not append content from response (Out of Band Swaps and Response Headers will still be processed).
    let none = NoSwap

module SwapOob =
    /// Enable out-of-band swap.
    let true' = SwapTrue

    /// The default, puts the content inside the target element.
    let innerHTML = SwapOption InnerHTML

    /// Replaces the entire target element with the returned content.
    let outerHTML = SwapOption OuterHTML

    /// Prepends the content before the first child inside the target.
    let beforebegin = SwapOption BeforeBegin

    /// Prepends the content before the target in the target's parent element.
    let afterbegin = SwapOption AfterBegin

    /// Appends the content after the last child inside the target.
    let beforeend = SwapOption BeforEend

    /// Appends the content after the target in the target's parent element.
    let afterend = SwapOption AfterEnd

    /// Deletes the target element regardless of the response.
    let delete = SwapOption Delete

    /// Does not append content from response (Out of Band Swaps and Response Headers will still be processed).
    let none = SwapOption NoSwap

    let innerHTMLTarget target = SwapOptionSelect(InnerHTML, target)
    let outerHTMLTarget target = SwapOptionSelect(OuterHTML, target)
    let beforebeginTarget target = SwapOptionSelect(BeforeBegin, target)
    let afterbeginTarget target = SwapOptionSelect(AfterBegin, target)
    let beforeendTarget target = SwapOptionSelect(BeforEend, target)
    let afterendTarget target = SwapOptionSelect(AfterEnd, target)
    let deleteTarget target = SwapOptionSelect(Delete, target)
    let noneTarget target = SwapOptionSelect(NoSwap, target)

module Sync =
    /// Drop (ignore) this request if an existing request is in flight (the default).
    let drop = Drop

    /// Drop (ignore) this request if an existing request is in flight, and, if that is not the case, abort this request if another request occurs while it is still in flight.
    let abort = Abort

    /// Abort the current request, if any, and replace it with this request.
    let replace = Replace

    /// Place this request in the request queue associated with the given element.
    let queue = Queue Default

    /// Queue the first request to show up while a request is in flight.
    let queueFirst = Queue First

    /// Queue the last request to show up while a request is in flight.
    let queueLast = Queue Last

    /// Queue all requests that show up while a request is in flight.
    let queueAll = Queue All

module Url =
    let true' = True
    let false' = False
    let path url' = Url url'

module Param =
    let all = AllParam
    let none = NoParams
    let exclude names = ExcludeParam names
    let include' names = IncludeParam names

module Disinherit =
    let all = AllAttributes
    let exclude names = ExcludeAttributes names

// ------------
// AJAX
// ------------

/// Issues a GET request to the given URL
let get (uri: string) =
    Attr.create "hx-get" uri

/// Issues a POST request to the given URL
let post (uri: string) =
    Attr.create "hx-post" uri

/// Issues a PUT request to the given URL
let put (uri: string) =
    Attr.create "hx-put" uri

/// Issues a PATCH request to the given URL
let patch (uri: string) =
    Attr.create "hx-patch" uri

/// Issues a DELETE request to the given URL
let delete (uri: string) =
    Attr.create "hx-delete" uri

// ------------
// Commmon Attributes
// ------------

/// Add or remove progressive enhancement for links and forms
let boost (enabled: bool) =
    Attr.create "hx-boost" (if enabled then "true" else "false")

/// Pushes the URL into the browser location bar, creating a new history entry
let pushUrl (option: UrlOption) =
    Attr.create "hx-push-url" (UrlOption.AsString option)

/// Select content to swap in from a response
let select (option: HxTarget) =
    Attr.create "hx-select" (HxTarget.AsString option)

/// Select content to swap in from a response, out of band (somewhere other than the target)
let selectOob (option: HxTarget) =
    Attr.create "hx-select-oob" (HxTarget.AsString option)

/// Controls how content is swapped in (outerHTML, beforeEnd, afterend, ...)
let swap (option: HxSwap) =
    Attr.create "hx-swap" (HxSwap.AsString option)

/// Marks content in a response to be out of band (should swap in somewhere other than the target)
let swapOob (option: SwapOobOption) =
    Attr.create "hx-swap-oob" (SwapOobOption.AsString option)

/// Specifies the target element to be swapped
let target (option: HxTarget) =
    Attr.create "hx-target" (HxTarget.AsString option)

/// Specifies the event that triggers the request
let trigger (options: HxTrigger list) =
    options
    |> List.map HxTrigger.AsString
    |> fun x -> String.Join(", ", x)
    |> Attr.create "hx-trigger"

/// Adds values to the parameters to submit with the request (JSON-formatted)
let vals input =
    input
    |> fun x -> JsonSerializer.Serialize(x)
    |> Attr.create "hx-vals"

/// Adds values to the parameters to submit with the request (JSON-formatted)
let valsJs json =
    Attr.create
        "hx-vals"
        (String.Concat [ "js:"; json ])

// ------------
// Additional Attributes
// ------------

/// Shows a confim() dialog before issuing a request
let confirm =
    Attr.create "hx-confirm"

/// Disables htmx processing for the given node and any children nodes
let disable =
    Attr.createBool "hx-disable"

/// Control and disable automatic attribute inheritance for child nodes
let disinherit (option: DisinheritOption) =
    Attr.create "hx-disinherit" (DisinheritOption.AsString option)

/// Changes the request encoding type
let encoding =
    Attr.create "hx-encoding"

/// Extensions to use for this element
let ext =
    Attr.create "hx-ext"

/// Adds to the headers that will be submitted with the request
let headers (values: (string * string) list) =
    values
    |> Map.ofList
    |> fun x -> JsonSerializer.Serialize(x)
    |> Attr.create "hx-headers"

/// The element to snapshot and restore during history navigation
let historyElt =
    Attr.createBool "hx-history-elt"

/// Include additional data in requests
let include' (values: HxTarget list) =
    values
    |> List.map HxTarget.AsString
    |> fun x -> String.Join(", ", x)
    |> Attr.create "hx-include"

/// The element to put the htmx-request class on during the request
let indicator (option: HxTarget) =
    Attr.create "hx-indicator" (HxTarget.AsString option)

/// Filters the parameters that will be submitted with a request
let params' (option: ParamOption) =
    Attr.create "hx-params" (ParamOption.AsString option)

/// Specifies elements to keep unchanged between requests
let preserve =
    Attr.createBool "hx-preserve"

/// Shows a prompt() before submitting a request
let prompt =
    Attr.create "hx-prompt"

/// Replace the URL in the browser location bar
let replaceUrl (option: UrlOption) =
    Attr.create "hx-replace-url" (UrlOption.AsString option)

/// Configures various aspects of the request
let request =
    Attr.create "hx-request"

/// Control how requests made be different elements are synchronized
let sync (targetOption: HxTarget, syncOption: SyncOption option) =
    let attrValue =
        let target' = HxTarget.AsString targetOption

        match syncOption with
        | Some sync' ->
            String.Concat(
                [
                    target'
                    ":"
                    SyncOption.AsString sync'
                ]
            )
        | None -> target'

    Attr.create "hx-sync" attrValue

/// Whether to force elements to validate themselves before a request
let validate (value: bool) =
    Attr.create "hx-validate" (string value)

/// Whether to save sensitive data to the history cache
let history (value: bool) =
    Attr.create "hx-history" (string value)

/// Handle any event with a script inline
let on eventName =
    Attr.create $"hx-on:{eventName}"

/// adds the `disabled` attribute to the specified elements while a request is in flight
let disabledElement (targetOption: HxTarget) =
    Attr.create "hx-disabled-elt" (HxTarget.AsString targetOption)
