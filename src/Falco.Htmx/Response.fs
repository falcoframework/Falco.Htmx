namespace Falco.Htmx

open Falco

/// Context for the HX-Location Response Header
type HxLocation(?event, ?source, ?handler, ?target, ?swap, ?values, ?headers) =
    /// The source element of the request
    member _.Source: HxTarget option = source
    /// An event that "triggered" the request
    member _.Event: string option = event
    /// A callback that will handle the response HTML
    member _.Handler: string option = handler
    /// The target to swap the response into
    member _.Target: HxTarget option = target
    /// How the response will be swapped in relative to the target
    member _.Swap: HxSwap option = swap
    /// Values to submit with the request
    member _.Values: (string * string) list = defaultArg values []
    /// Headers to submit with the request
    member _.Headers: (string * string) list = defaultArg headers []

/// Value for the HX-Trigger Response Header
type HxTriggerResponse =
    /// A list of event names to trigger
    | Events of string list
    /// A list of event names and associated data to trigger
    | DetailedEvents of (string * obj) list

[<RequireQualifiedAccess>]
module Response =
    open System.Text.Json

    [<Literal>]
    let private _trueValue = "true"

    /// Allows you to do a client-side redirect that does not do a full page reload
    let withHxLocationOptions (path: string, ctx: HxLocation option) : HttpResponseModifier =
        let headerValue =
            match ctx with
            | None -> path
            | Some ctx' ->
                [
                    "path", path
                    "source", Option.map HxTarget.AsString ctx'.Source |> Option.defaultValue ""
                    "event", Option.defaultValue "" ctx'.Event
                    "handler", Option.defaultValue "" ctx'.Handler
                    "target", Option.map HxTarget.AsString ctx'.Target |> Option.defaultValue ""
                    "swap", Option.map HxSwap.AsString ctx'.Swap |> Option.defaultValue ""
                ]
                |> Map.ofList
                |> fun x -> JsonSerializer.Serialize(x)

        Response.withHeaders [ "HX-Location", headerValue ]

    /// Allows you to do a client-side redirect that does not do a full page reload. Alias
    /// for withHxLocationOptions with None context.
    let withHxLocation (path: string) : HttpResponseModifier =
        withHxLocationOptions (path, None)

    /// Pushes a new url into the history stack
    let withHxPushUrl (url: string) : HttpResponseModifier =
        Response.withHeaders [ "HX-Push-Url", url ]

    /// Can be used to do a client-side redirect to a new location
    let withHxRedirect (url: string) : HttpResponseModifier =
        Response.withHeaders [ "HX-Redirect", url ]

    /// If set to "true" the client side will do a a full refresh of the page
    let withHxRefresh: HttpResponseModifier =
        Response.withHeaders [ "HX-Refresh", _trueValue ]

    /// Replaces the current URL in the location bar
    let withHxReplaceUrl (url: string) : HttpResponseModifier =
        Response.withHeaders [ "HX-Replace-Url", url ]

    /// Allows you to specify how the response will be swapped. See hx-swap for possible values
    let withHxReswap (option: HxSwap) =
        Response.withHeaders [ "HX-Reswap", HxSwap.AsString option ]

    /// A CSS selector that updates the target of the content update to a different element on the page
    let withHxRetarget (option: HxTarget) =
        Response.withHeaders [ "HX-Retarget", HxTarget.AsString option ]

    /// Allows you to trigger client side events, see the documentation for more info
    let withHxTrigger<'T> (triggerResponse: HxTriggerResponse) : HttpResponseModifier =
        let headerValue =
            match triggerResponse with
            | Events events -> events |> String.concat ", "
            | DetailedEvents events ->
                events
                |> Map.ofList
                |> fun x -> JsonSerializer.Serialize(x)

        Response.withHeaders [ "HX-Trigger", headerValue ]

    /// Allows you to trigger client side events, see the documentation for more info
    let withHxTriggerAfterSettle: HttpResponseModifier =
        Response.withHeaders [ "HX-Trigger-After-Settle", _trueValue ]

    /// Allows you to trigger client side events, see the documentation for more info
    let withHxTriggerAfterSwap: HttpResponseModifier =
        Response.withHeaders [ "HX-Trigger-After-Swap", _trueValue ]

    /// A CSS selector that allows you to choose which part of the response is used to be swapped in.
    /// see the documentation for more info
    let withHxReselect (selector: string) : HttpResponseModifier =
        Response.withHeaders [ "HX-Reselect", selector ]
