namespace Falco.Htmx

open System
open System.Text.Json
open Falco.Markup

/// javascript fatigue:
/// longing for a hypertext
/// already in hand
type Hx =

    // ------------
    // hx-[get|post|put|patch|delete]
    // ------------

    /// Issues a GET request to the given URL
    static member get (uri: string) =
        Attr.create "hx-get" uri

    /// Issues a POST request to the given URL
    static member post (uri: string) =
        Attr.create "hx-post" uri

    /// Issues a PUT request to the given URL
    static member put (uri: string) =
        Attr.create "hx-put" uri

    /// Issues a PATCH request to the given URL
    static member patch (uri: string) =
        Attr.create "hx-patch" uri

    /// Issues a DELETE request to the given URL
    static member delete (uri: string) =
        Attr.create "hx-delete" uri


    // ------------
    // hx-trigger
    // ------------

    /// Specify the discrete trigger event.
    static member trigger (event: string, ?modifiers : HxTrigger list, ?filter : string) =
        let makeFilterString (filter: string) =
            String.Concat("[", filter, "]")

        let makeModifierString (modifiers: HxTrigger list) =
            modifiers
            |> List.map HxTrigger.AsString
            |> (fun x -> String.Join(" ", x))

        match filter,modifiers with
        | None, None -> event
        | Some filter, None -> String.Concat(event,makeFilterString filter)
        | None, Some modifiers -> String.Concat(event," ",makeModifierString modifiers)
        | Some filter, Some modifiers -> String.Concat(event,makeFilterString filter," ",makeModifierString modifiers)
        |> Attr.create "hx-trigger"


    // ------------
    // hx-target
    // ------------

    /// Specifies the target element to be swapped
    static member target (hxTarget : HxTarget) =
        Attr.create "hx-target" (HxTarget.AsString hxTarget)

    /// Indicates that the element that the hx-target attribute is on is the
    /// target.
    static member targetThis =
        Hx.target HxTarget.This

    /// A CSS query selector of the element to target.
    static member targetCss (selector : string) =
        Hx.target (HxTarget.Css selector)

    /// Find the first child descendant element that matches the given CSS
    /// selector.
    static member targetFind (selector : string) =
        Hx.target (HxTarget.Find selector)

    /// Find the closest ancestor element or itself, that matches the given CSS
    /// selector
    static member targetClosest (selector : string) =
        Hx.target (HxTarget.Closest selector)

    /// Scan forward for the first element that matches the given CSS selector.
    static member targetNext (selector : string) =
        Hx.target (HxTarget.Next selector)

    /// Scan forward for element.nextElementSibling when no selector is
    /// specified.
    static member targetNextSibling =
        Hx.target HxTarget.NextSibling

    /// Scan backward for the first element that matches the given CSS selector.
    static member targetPrevious (selector : string) =
        Hx.target (HxTarget.Previous selector)

    /// Scan backward for element.previousElementSibling when no selector is
    /// specified.
    static member targetPreviousSibling =
        Hx.target HxTarget.PreviousSibling


    // ------------
    // hx-swap
    // ------------

    /// Controls how content is swapped in (outerHTML, beforeEnd, afterend, ...)
    static member swap (hxSwap : HxSwap) =
        Attr.create "hx-swap" (HxSwap.AsString hxSwap)

    /// The default, puts the content inside the target element.
    static member swapInnerHtml =
        Hx.swap HxSwap.InnerHTML

    /// Replaces the entire target element with the returned content.
    static member swapOuterHtml =
        Hx.swap HxSwap.OuterHTML

    /// Prepends the content before the first child inside the target.
    static member swapBeforeBegin =
        Hx.swap HxSwap.BeforeBegin

    /// Prepends the content before the target in the target's parent element.
    static member swapAfterBegin =
        Hx.swap HxSwap.AfterBegin

    /// Appends the content after the last child inside the target.
    static member swapBeforeEnd =
        Hx.swap HxSwap.BeforeEnd

    /// Appends the content after the target in the target's parent element.
    static member swapAfterEnd =
        Hx.swap HxSwap.AfterEnd

    /// Deletes the target element regardless of the response.
    static member swapDelete =
        Hx.swap HxSwap.Delete

    /// Does not append content from response (Out of Band Swaps and Response
    /// Headers will still be processed).
    static member swapNone =
        Hx.swap HxSwap.NoSwap


    // ------------
    // hx-swap-oob
    // ------------

    /// Add or remove progressive enhancement for links and forms
    static member swapOob (hxSwap : HxSwap, ?selector : string) =
        let createAttr = Attr.create "hx-swap-oob"
        let swapValue = HxSwap.AsString hxSwap

        match selector with
        | Some x when not(String.IsNullOrWhiteSpace(x)) ->
            createAttr (String.Concat(swapValue, ":", x))
        | _ ->
            createAttr swapValue

    /// Enable progressive enhancement for links and forms
    static member swapOobOn =
        Hx.swapOob HxSwap.OuterHTML


    // ------------
    // hx-select
    // ------------

    /// Select the content you want swapped from a response.
    static member select selector =
        Attr.create "hx-select" selector


    // ------------
    // hx-select-oob
    // ------------

    /// Select content from a response to be swapped in via an out-of-band swap.
    static member selectOob (selectors : string seq) =
        Attr.create "hx-select-oob" (String.Join(",", selectors))

    /// Select content from a response to be swapped in via an out-of-band swap.
    static member selectOob (selector : string) =
        Hx.selectOob (seq { selector })

    /// Select content from a response to be swapped in via an out-of-band swap.
    static member selectOob (selectors : (string * HxSwap) seq) =
        selectors
        |> Seq.map (fun (selector, swap) ->
            String.Concat(selector, ":", HxSwap.AsString swap))
        |> String.concat ","
        |> Attr.create "hx-select-oob"


    // ------------
    // hx-boost
    // ------------

    /// Add or remove progressive enhancement for links and forms
    static member boost (enabled: bool) =
        Attr.create "hx-boost" (if enabled then "true" else "false")

    /// Enable progressive enhancement for links and forms
    static member boostOn =
        Hx.boost true

    /// Disable progressive enhancement for links and forms
    static member boostOff =
        Hx.boost false


    // ------------
    // hx-push-url
    // ------------


    /// Pushes the specified URL into the browser location bar, creating a new
    /// history entry.
    static member pushUrl (uri : string) =
        Attr.create "hx-push-url" uri

    /// Pushes the URL into the browser location bar, creating a new history
    /// entry.
    static member pushUrl (enabled : bool) =
        Hx.pushUrl (if enabled then "true" else "false")

    /// Pushes the URL into the browser location bar, creating a new history
    /// entry.
    static member pushUrlOn =
        Hx.pushUrl true

    /// Prevents the URL from being pushed into the browser location bar.
    static member pushUrlOff =
        Hx.pushUrl false


    // ------------
    // hx-sync
    // ------------

    /// Synchronize AJAX requests between multiple elements.
    static member sync (targetOption : HxTarget, ?syncOption : HxSync) =
        match syncOption with
        | Some hxSync -> String.Concat(HxTarget.AsString targetOption, ":", HxSync.AsString hxSync)
        | None -> HxTarget.AsString targetOption
        |> Attr.create "hx-sync"

    // ------------
    // hx-include
    // ------------

    /// Include additional element values in an AJAX request.
    static member include' (hxTarget : HxTarget) =
        Attr.create "hx-include" (HxTarget.AsString hxTarget)

    /// Include the current element in the request
    static member includeThis =
        Hx.include' HxTarget.This

    /// Include CSS query selector of the element to include.
    static member includeCss (selector : string) =
        Hx.include' (HxTarget.Css selector)

    /// Include the first child descendant element that matches the given CSS
    /// selector.
    static member includeFind (selector : string) =
        Hx.include' (HxTarget.Find selector)

    /// Include the closest ancestor element or itself, that matches the given
    /// CSS selector
    static member includeClosest (selector : string) =
        Hx.include' (HxTarget.Closest selector)

    /// Include forward for the first element that matches the given CSS
    /// selector.
    static member includeNext (selector : string) =
        Hx.include' (HxTarget.Next selector)

    /// Include forward for element.nextElementSibling when no selector is
    /// specified.
    static member includeNextSibling =
        Hx.include' HxTarget.NextSibling

    /// Include backward for the first element that matches the given CSS
    /// selector.
    static member includePrevious (selector : string) =
        Hx.include' (HxTarget.Previous selector)

    /// Include backward for element.previousElementSibling when no selector is
    /// specified.
    static member includePreviousSibling =
        Hx.include' HxTarget.PreviousSibling


    // ------------
    // hx-params
    // ------------

    /// Filter the parameters that will be submitted with an AJAX request.
    static member params' (hxParams : HxParams) =
        Attr.create "hx-params" (HxParams.AsString hxParams)

    /// Include all parameters.
    static member paramsAll =
        Hx.params' HxParams.All

    /// Include no parameters.
    static member paramsNone =
        Hx.params' HxParams.NoParams

    ///  Include all but parameters specified.
    static member paramsExclude exclude =
        Hx.params' (HxParams.Exclude exclude)

    /// Include all parameters specified.
    static member paramsInclude include' =
        Hx.params' (HxParams.Include include')


    // ------------
    // hx-vals
    // ------------

    /// Add values to submit with the request (JSON format)
    static member vals json =
        Attr.create "hx-vals" json

    /// Add values to submit with the request (JS format)
    static member valsJs js =
        Hx.vals (String.Concat("js:", js))


    // ------------
    // hx-confirm
    // ------------

    /// Shows a confirm() dialog before issuing a request
    static member confirm text =
        Attr.create "hx-confirm" text


    // ------------
    // hx-disable
    // ------------

    /// Disables htmx processing for the given node and any children nodes
    static member disable =
        Attr.createBool "hx-disable"


    // ------------
    // hx-disabled-elt
    // ------------

    /// Adds the disabled attribute to the specified elements while a request is
    /// in flight
    static member disabled (hxTarget : HxTarget) =
        Attr.create "hx-disabled-elt" (HxTarget.AsString hxTarget)

    /// Include the current element in the request
    static member disabledThis =
        Hx.disabled HxTarget.This

    /// Include CSS query selector of the element to include.
    static member disabledCss (selector : string) =
        Hx.disabled (HxTarget.Css selector)

    /// Include the first child descendant element that matches the given CSS
    /// selector.
    static member disabledFind (selector : string) =
        Hx.disabled (HxTarget.Find selector)

    /// Include the closest ancestor element or itself, that matches the given
    /// CSS selector
    static member disabledClosest (selector : string) =
        Hx.disabled (HxTarget.Closest selector)

    /// Include forward for the first element that matches the given CSS
    /// selector.
    static member disabledNext (selector : string) =
        Hx.disabled (HxTarget.Next selector)

    /// Include forward for element.nextElementSibling when no selector is
    /// specified.
    static member disabledNextSibling =
        Hx.disabled HxTarget.NextSibling

    /// Include backward for the first element that matches the given CSS
    /// selector.
    static member disabledPrevious (selector : string) =
        Hx.disabled (HxTarget.Previous selector)

    /// Include backward for element.previousElementSibling when no selector is
    /// specified.
    static member disabledPreviousSibling =
        Hx.disabled HxTarget.PreviousSibling


    // ------------
    // hx-inherit
    // ------------

    /// Control and enable automatic attribute inheritance for child nodes if it
    /// has been disabled by default.
    static member inherit' (attributes : string) =
        Attr.create "hx-inherit" attributes

    /// Control and enable automatic attribute inheritance for child nodes if it
    /// has been disabled by default.
    static member inheritAll =
        Hx.inherit' "*"


    // ------------
    // hx-disinherit
    // ------------

    /// Control and disable automatic attribute inheritance for child nodes.
    static member disinherit (attributes : string) =
        Attr.create "hx-disinherit" attributes

    /// Control and disable automatic attribute inheritance for child nodes.
    static member disinheritAll =
        Hx.disinherit "*"


    // ------------
    // hx-encoding
    // ------------

    /// Changes the request encoding type to multipart/form-data.
    static member encodingMultipart =
        Attr.create "hx-encoding" "multipart/form-data"


    // ------------
    // hx-ext
    // ------------

    /// Extensions to use for this element.
    static member ext (extensions : string) =
        Attr.create "hx-ext" extensions

    /// Extensions to ignore for this element.
    static member extIgnore (extensions : string) =
        Hx.ext (String.Concat("ignore:", extensions))


    // ------------
    // hx-headers
    // ------------

    /// Adds to the headers that will be submitted with the request.
    static member headers (values : (string * string) list, ?evaluate : bool) =
        values
        |> Map.ofList
        |> fun x ->
            let json = JsonSerializer.Serialize(x)
            if defaultArg evaluate false then
                String.Concat("js:", json)
            else
                json
        |> Attr.create "hx-headers"


    // ------------
    // hx-history
    // ------------

    /// Prevent sensitive data being saved to the history cache.
    static member historyOff =
        Attr.create "hx-history" "false"


    // ------------
    // hx-history-elt
    // ------------

    /// The element to snapshot and restore during history navigation.
    static member historyElt =
        Attr.createBool "hx-history-elt"


    // ------------
    // hx-indicator
    // ------------

    /// The element to put the htmx-request class on during the request.
    static member indicator (selector : string) =
        Attr.create "hx-indicator" selector

    /// The closest element to put the htmx-request class on during the request.
    static member indicatorClosest (selector : string) =
        Hx.indicator (String.Concat("closest ", selector))


    // -----------
    // hx-preserve
    // -----------

    /// Specifies elements to keep unchanged between requests.
    static member preserve =
        Attr.createBool "hx-preserve"


    // -----------
    // hx-prompt
    // -----------

    /// Shows a prompt() before submitting a request.
    static member prompt (message : string) =
        Attr.create "hx-prompt" message


    // -----------
    // hx-replace-url
    // -----------

    /// Replace the URL in the browser location bar with the specified.
    static member replaceUrl (url : string) =
        Attr.create "hx-replace-url" url

    /// Replace the URL in the browser location bar.
    static member replaceUrl (enabled : bool) =
        Hx.replaceUrl (if enabled then "true" else "false")

    /// Replace the URL in the browser location bar.
    static member replaceUrlOn =
        Hx.replaceUrl true

    /// Do not replace the URL in the browser location bar.
    static member replaceUrlOff =
        Hx.replaceUrl false


    // -----------
    // hx-validate
    // -----------

    /// Force elements to validate themselves before a request.
    static member validate =
        Attr.create "hx-validate" "true"


    // -----------
    // hx-request
    // -----------

    /// Configure various aspects of the request.
    static member request (
        ?timeout : int,
        ?credentials : bool,
        ?noHeaders : bool,
        ?evaluate : bool) =
        let json =
            let values =
                String.Join(",", seq {
                    match timeout with
                    | Some x when x > 0 -> yield sprintf "\"timeout\":%i" x
                    | _ -> ()

                    match credentials with
                    | Some x when x -> yield "\"credentials\":true"
                    | _ -> ()

                    match noHeaders with
                    | Some x when x -> yield "\"noHeaders\":true"
                    | _ -> () })

            String.Concat("{", values,"}")

        let requestValue =
            if defaultArg evaluate false then
                String.Concat("js:", json)
            else
                json

        Attr.create "hx-request" requestValue
