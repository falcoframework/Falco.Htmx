namespace Falco.Htmx.Tests

open Falco.Htmx
open FsUnit.Xunit
open Microsoft.AspNetCore.Http
open NSubstitute
open Xunit

module ResponseTests =
    let private getHttpContextWriteable () =
        let ctx = Substitute.For<HttpContext>()

        let req = Substitute.For<HttpRequest>()
        req.Headers.Returns(Substitute.For<HeaderDictionary>()) |> ignore

        ctx

    [<Fact>]
    let ``withHxLocation sets the HX-Location header with options`` () =
        let path = "/new-location"
        let ctx = HxLocation(event = "click", source = HxTarget.This)
        let modifier = Response.withHxLocationOptions (path, Some ctx)
        let httpCtx = getHttpContextWriteable ()

        modifier httpCtx |> ignore

        let headerValue = httpCtx.Response.Headers.["HX-Location"].ToString()
        headerValue.Contains(path) |> should be True
        headerValue.Contains("\"event\":\"click\"") |> should be True
        headerValue.Contains("\"source\":\"this\"") |> should be True

    [<Fact>]
    let ``withHxLocation sets the HX-Location header`` () =
        let path = "/new-location"
        let ctx = HxLocation(event = "click", source = HxTarget.This)
        let modifier = Response.withHxLocation path
        let httpCtx = getHttpContextWriteable ()

        modifier httpCtx |> ignore

        let headerValue = httpCtx.Response.Headers.["HX-Location"].ToString()
        headerValue.Contains(path) |> should be True

    [<Fact>]
    let ``withHxPushUrl sets the HX-Push-Url header`` () =
        let url = "/new-url"
        let modifier = Response.withHxPushUrl url
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Push-Url"].ToString() |> should equal url

    [<Fact>]
    let ``withHxRedirect works`` () =
        let url = "/redirect-url"
        let modifier = Response.withHxRedirect url
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Redirect"].ToString() |> should equal url

    [<Fact>]
    let ``withHxRefresh works`` () =
        let modifier = Response.withHxRefresh
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Refresh"].ToString() |> should equal "true"


    [<Fact>]
    let ``withHxReplaceUrl works`` () =
        let url = "/replace-url"
        let modifier = Response.withHxReplaceUrl url
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Replace-Url"].ToString() |> should equal url

    [<Fact>]
    let ``withHxReswap works`` () =
        let swap = HxSwap.AfterEnd
        let modifier = Response.withHxReswap swap
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Reswap"].ToString() |> should equal (HxSwap.AsString swap)

    [<Fact>]
    let ``withHxRetarget works`` () =
        let target = HxTarget.This
        let modifier = Response.withHxRetarget target
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Retarget"].ToString() |> should equal (HxTarget.AsString target)

    [<Fact>]
    let ``withHxTrigger works`` () =
        let events = [ "event1"; "event2" ]
        let modifier = Response.withHxTrigger (Events events)
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Trigger"].ToString() |> should equal (String.concat ", " events)

    [<Fact>]
    let ``withHxTriggerAfterSettle works`` () =
        let modifier = Response.withHxTriggerAfterSettle
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Trigger-After-Settle"].ToString() |> should equal "true"

    [<Fact>]
    let ``withHxTriggerAfterSwap works`` () =
        let modifier = Response.withHxTriggerAfterSwap
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Trigger-After-Swap"].ToString() |> should equal "true"

    [<Fact>]
    let ``withHxReselect works`` () =
        let selector = "#my-element"
        let modifier = Response.withHxReselect selector
        let ctx = getHttpContextWriteable ()

        modifier ctx |> ignore

        ctx.Response.Headers.["HX-Reselect"].ToString() |> should equal selector
