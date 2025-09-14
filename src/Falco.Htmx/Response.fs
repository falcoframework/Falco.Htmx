namespace Falco.Htmx

open Falco

[<RequireQualifiedAccess>]
module Response =
    open System.Text.Json

    [<Literal>]
    let private _trueValue = "true"

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
