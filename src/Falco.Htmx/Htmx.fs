namespace Falco.Htmx

open System
open FSharp.Core

/// The hx-target attribute allows you to target a different element for
/// swapping than the one issuing the AJAX request.
type HxTarget =
    internal
    | This
    | NextSibling
    | PreviousSibling
    | CssSelector of string
    | Closest of string
    | Find of string
    | Next of string
    | Previous of string

    static member internal AsString(x: HxTarget) =
        match x with
        | This -> "this"
        | NextSibling -> "next"
        | PreviousSibling -> "previous"
        | CssSelector selector -> selector
        | Closest selector -> String.Concat [ "closest "; selector ]
        | Find selector -> String.Concat [ "find "; selector ]
        | Next selector -> String.Concat [ "next "; selector ]
        | Previous selector -> String.Concat [ "previous "; selector ]

///
type TimingDeclaration =
    internal
    | Milliseconds of float
    | Seconds of float
    | Minutes of float

    static member AsString(x: TimingDeclaration) =
        match x with
        | Milliseconds ms -> sprintf "%fms" ms
        | Seconds s -> sprintf "%fs" s
        | Minutes m -> sprintf "%fm" m

/// Determines how events are queued if an event occurs while a request for
/// another event is in flight
type QueueOption =
    | First
    | Last
    | All
    | NoQueue

    static member internal AsString(x: QueueOption) =
        match x with
        | First -> "first"
        | Last -> "last"
        | All -> "all"
        | NoQueue -> "none"

/// Standard events can have modifiers that change how they behave
type EventModifier =
    internal
    | Once
    | Changed
    | Delay of TimingDeclaration
    | Throttle of TimingDeclaration
    | From
    | Target of HxTarget
    | Consume
    | Queue of QueueOption

    static member AsString(x: EventModifier) =
        match x with
        | Once -> "once"
        | Changed -> "changed"
        | Delay timing -> String.Concat [ "delay:"; TimingDeclaration.AsString timing ]
        | Throttle timing -> String.Concat [ "throttle:"; TimingDeclaration.AsString timing ]
        | From -> "from"
        | Target selector -> String.Concat [ "target:"; HxTarget.AsString selector ]
        | Consume -> "consume"
        | Queue queue -> String.Concat [ "queue:"; QueueOption.AsString queue ]

/// The hx-trigger attribute allows you to specify what triggers an AJAX request.
type HxTrigger =
    internal
    | Event of string * string option * EventModifier list
    | Poll of TimingDeclaration

    static member AsString(x: HxTrigger) =
        let makeFilterString (filter: string) =
            String.Concat [ "["; filter; "]" ]

        let makeModifierString (modifiers: EventModifier list) =
            modifiers
            |> List.map EventModifier.AsString
            |> (fun x -> String.Join(" ", x))

        match x with
        | Poll timing -> TimingDeclaration.AsString timing
        | Event(name, None, []) -> name
        | Event(name, Some filter, []) -> String.Concat [ name; makeFilterString filter ]
        | Event(name, None, modifiers) -> String.Concat [ name; makeModifierString modifiers ]
        | Event(name, Some filter, modifiers) -> String.Concat [ name; makeFilterString filter; makeModifierString modifiers ]

/// The hx-swap attribute allows you to specify how the response will be swapped
/// in relative to the target of an AJAX request.
type HxSwap =
    internal
    | InnerHTML
    | OuterHTML
    | BeforeBegin
    | AfterBegin
    | BeforEend
    | AfterEnd
    | Delete
    | NoSwap

    static member internal AsString(x: HxSwap) =
        match x with
        | InnerHTML -> "innerHTML"
        | OuterHTML -> "outerHTML"
        | BeforeBegin -> "beforebegin"
        | AfterBegin -> "afterbegin"
        | BeforEend -> "beforeend"
        | AfterEnd -> "afterend"
        | Delete -> "delete"
        | NoSwap -> "none"

type SwapOobOption =
    internal
    | SwapTrue
    | SwapOption of HxSwap
    | SwapOptionSelect of HxSwap * HxTarget

    static member internal AsString(x: SwapOobOption) =
        match x with
        | SwapTrue -> "true"
        | SwapOption swap -> HxSwap.AsString swap
        | SwapOptionSelect(swap, selector) -> String.Concat [ HxSwap.AsString swap; ":"; HxTarget.AsString selector ]

/// The hx-sync attribute allows you to synchronize AJAX requests between
/// multiple elements.
///
/// The hx-sync attribute consists of a CSS selector to indicate the element to
/// synchronize on, followed optionally by a colon and then by an optional
/// syncing strategy.
type SyncQueueOption =
    internal
    | First
    | Last
    | All
    | Default

    static member internal AsString(x: SyncQueueOption) =
        match x with
        | First -> "first"
        | Last -> "last"
        | All -> "all"
        | Default -> ""

type SyncOption =
    internal
    | Drop
    | Abort
    | Replace
    | Queue of SyncQueueOption

    static member internal AsString(x: SyncOption) =
        match x with
        | Drop -> "drop"
        | Abort -> "abort"
        | Replace -> "replace"
        | Queue queue ->
            match queue with
            | Default -> "queue"
            | x -> String.Concat [ "queue "; SyncQueueOption.AsString x ]

type UrlOption =
    internal
    | True
    | False
    | Url of string

    static member internal AsString(x: UrlOption) =
        match x with
        | True -> "true"
        | False -> "false"
        | Url url -> url

/// The hx-params attribute allows you to filter the parameters that will be
/// submitted with an AJAX request.
type ParamOption =
    internal
    | AllParam
    | NoParams
    | ExcludeParam of string list
    | IncludeParam of string list

    static member internal AsString(x: ParamOption) =
        match x with
        | AllParam -> "*"
        | NoParams -> "none"
        | ExcludeParam names -> String.Concat [ "not "; String.Join(", ", names) ]
        | IncludeParam names -> String.Join(", ", names)

/// The hx-disinherit attribute allows you to control this automatic attribute
/// inheritance. An example scenario is to allow you to place an hx-boost on the
/// body element of a page, but overriding that behavior in a specific part of
/// the page to allow for more specific behaviors.
type DisinheritOption =
    internal
    | AllAttributes
    | ExcludeAttributes of string list

    static member internal AsString(x: DisinheritOption) =
        match x with
        | AllAttributes -> "*"
        | ExcludeAttributes [] -> "*"
        | ExcludeAttributes names -> String.Join(" ", names)

module HtmxScript =
    /// The unpkg URL for htmx
    let cdnSrc = "https://unpkg.com/htmx.org"
