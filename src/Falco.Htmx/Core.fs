namespace Falco.Htmx

open System

module HtmxScript =
    /// The CDN URL for htmx
    let cdnSrc = "https://unpkg.com/htmx.org"

/// Specify how the response will be swapped in relative to the target of an
/// AJAX request.
type HxSwap =
    | InnerHTML
    | OuterHTML
    | BeforeBegin
    | AfterBegin
    | BeforeEnd
    | AfterEnd
    | Delete
    | NoSwap

    static member AsString hxSwap =
        match hxSwap with
        | HxSwap.InnerHTML -> "innerHTML"
        | HxSwap.OuterHTML -> "outerHTML"
        | HxSwap.BeforeBegin -> "beforebegin"
        | HxSwap.AfterBegin -> "afterbegin"
        | HxSwap.BeforeEnd -> "beforeend"
        | HxSwap.AfterEnd -> "afterend"
        | HxSwap.Delete -> "delete"
        | HxSwap.NoSwap -> "none"

/// Target element for swapping than the one issuing the AJAX request.
type HxTarget =
    | This
    | NextSibling
    | PreviousSibling
    | Css of string
    | Closest of string
    | Find of string
    | Next of string
    | Previous of string

    static member AsString hxTarget =
        match hxTarget with
        | HxTarget.This -> "this"
        | HxTarget.NextSibling -> "next"
        | HxTarget.PreviousSibling -> "previous"
        | HxTarget.Css selector -> selector
        | HxTarget.Closest selector -> String.Concat (seq { "closest "; selector })
        | HxTarget.Find selector -> String.Concat (seq { "find "; selector })
        | HxTarget.Next selector -> String.Concat (seq { "next "; selector })
        | HxTarget.Previous selector -> String.Concat (seq { "previous "; selector })

/// Defines an amount of time at a specific interval.
type HxTime =
    | Ms of float
    | Sec of float
    | Min of float

    static member AsString(hxTime : HxTime) =
        match hxTime with
        | HxTime.Ms ms -> String.Concat (seq { ms.ToString("G3"); "ms" })
        | HxTime.Sec s -> String.Concat (seq { s.ToString("G3"); "s" })
        | HxTime.Min m -> String.Concat (seq { m.ToString("G3"); "m" })

/// Specifies the event that triggers the request
type HxTrigger =
    | Once
    | Changed
    | Delay of HxTime
    | Throttle of HxTime
    | FromWindow
    | FromDocument
    | From of HxTarget
    | Target of HxTarget
    | Consume
    | QueueFirst
    | QueueLast
    | QueueAll
    | QueueNone

    static member AsString hxTrigger =
        match hxTrigger with
        | HxTrigger.Once -> "once"
        | HxTrigger.Changed -> "changed"
        | HxTrigger.Delay hxTime -> String.Concat (seq { "delay:"; HxTime.AsString hxTime })
        | HxTrigger.Throttle hxtime -> String.Concat (seq { "throttle:"; HxTime.AsString hxtime })
        | HxTrigger.FromWindow -> "from:window"
        | HxTrigger.FromDocument -> "from:document"
        | HxTrigger.From hxTarget -> String.Concat (seq { "from:"; HxTarget.AsString hxTarget })
        | HxTrigger.Target hxTarget -> String.Concat (seq { "target:"; HxTarget.AsString hxTarget })
        | HxTrigger.Consume -> "consume"
        | HxTrigger.QueueFirst -> "queue:first"
        | HxTrigger.QueueLast -> "queue:last"
        | HxTrigger.QueueAll -> "queue:all"
        | HxTrigger.QueueNone -> "queue:none"

/// control how requests made by different elements are synchronized
type HxSync =
    | Drop
    | Abort
    | Replace
    | QueueFirst
    | QueueLast
    | QueueAll

    static member AsString hxSync =
        match hxSync with
        | Drop -> "drop"
        | Abort -> "abort"
        | Replace -> "replace"
        | QueueFirst -> "queue first"
        | QueueLast -> "queue last"
        | QueueAll -> "queue all"

/// Filters the parameters that will be submitted with a request
type HxParams =
    | All
    | NoParams
    | Exclude of string seq
    | Include of string seq

    static member internal AsString hxParams =
        match hxParams with
        | All-> "*"
        | NoParams -> "none"
        | Exclude names -> String.Concat (seq { "not "; String.Join(", ", names) })
        | Include names -> String.Join(", ", names)