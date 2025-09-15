namespace Falco.Htmx

open Falco
open Microsoft.AspNetCore.Http

/// htmx Request Headers
type HtmxRequestHeaders =
    { /// Indicates that the request is via an element using hx-boost
      HxBoosted: string option
      /// The current URL of the browser
      HxCurrentUrl: string option
      /// True if the request is for history restoration after a miss in the local history cache
      HxHistoryRestoreRequest: string option
      /// The user response to an hx-prompt
      HxPrompt: string option
      /// Always true
      HxRequest: string option
      /// The id of the target element if it exists
      HxTarget: string option
      /// The name of the triggered element if it exists
      HxTriggerName: string option
      /// The id of the triggered element if it exists
      HxTrigger: string option }

[<RequireQualifiedAccess>]
module Request =
    let getHtmxHeaders (ctx: HttpContext) : HtmxRequestHeaders =
        let headers = Request.getHeaders ctx

        { HxBoosted = headers.TryGetStringNonEmpty "HX-Boosted"
          HxCurrentUrl = headers.TryGetStringNonEmpty "HX-Current-URL"
          HxHistoryRestoreRequest = headers.TryGetStringNonEmpty "HX-History-Restore-Request"
          HxPrompt = headers.TryGetStringNonEmpty "HX-Prompt"
          HxRequest = headers.TryGetStringNonEmpty "HX-Request"
          HxTarget = headers.TryGetStringNonEmpty "HX-Target"
          HxTriggerName = headers.TryGetStringNonEmpty "HX-Trigger-Name"
          HxTrigger = headers.TryGetStringNonEmpty "HX-Trigger" }
