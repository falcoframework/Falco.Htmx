# Falco.Htmx

[![NuGet Version](https://img.shields.io/nuget/v/Falco.Htmx.svg)](https://www.nuget.org/packages/Falco.Htmx)
[![build](https://github.com/FalcoFramework/Falco.Htmx/actions/workflows/build.yml/badge.svg)](https://github.com/FalcoFramework/Falco.Htmx/actions/workflows/build.yml)

```fsharp
open Falco.Markup
open Falco.Htmx

let demo =
    _button
        [ Hx.get "/click-me"
          Hx.swapOuterHtml
          Hx.targetCss "#wrapper" ]
        [ _text "Reset" ]
```

[Falco.Htmx](https://github.com/FalcoFramework/Falco.Htmx) brings type-safe [htmx](https://htmx.org/) support to [Falco](https://github.com/FalcoFramework/Falco). It provides a complete mapping of all attributes, typed request data and ready-made response modifiers.

## Key Features

- [Idiomatic mapping](#htmx-attributes) of `htmx` attributes (i.e., `hx-get`, `hx-post`, `hx-target` etc.).
- Typed access to htmx request headers.
- Prepared response modifiers for common use-cases (i.e., `HX-location`, `HX-Push-Url`).
- Built-in [support](#template-fragments) for [template fragments](https://htmx.org/docs/#template-fragments).

## Design Goals

- Create a self-documenting way to integrate htmx into Falco applications.
- Match the specification of htmx as closely as possible, ideally one-to-one.
- Provide type safety without over-abstracting.

## Getting Started

This guide assumes you have a [Falco](https://github.com/FalcoFramework/Falco) project setup. If you don't, you can create a new Falco project using the following commands. The full code for this guide can be found in the [Hello World example](examples/HelloWorld/).

```shell
> dotnet new web -lang F# -o HelloWorld
> cd HelloWorldApp
```

Install the nuget package:

```shell
> dotnet add package Falco
> dotnet add package Falco.Htmx
```

Remove any `*.fs` files created automatically, create a new file named `Program.fs` and set the contents to the following:

```fsharp
open Falco
open Falco.Htmx
open Falco.Markup
open Falco.Routing
open Microsoft.AspNetCore.Builder

let bldr = WebApplication.CreateBuilder()
let wapp = bldr.Build()

let endpoints =
    [
    ]

wapp.UseRouting()
    .UseFalco(endpoints)
    .Run()
```

Now, let's incorporate htmx into our Falco application. First we'll define a simple route that returns a button that, when clicked, will swap the inner HTML of a target element with the response from a GET request.

```fsharp
let handleIndex : HttpHandler =
    let html =
        _html [] [
            _head [] [
                _script [ _src_ HtmxScript.cdnSrc ] [] ]
            _body [] [
                _h1' "Example: Hello World"
                _button
                    [ Hx.get "/click"
                      Hx.swapOuterHtml ]
                    [ _text "Click Me" ] ] ]

    Response.ofHtml html
```

Next, we'll define a handler for the click event that will return HTML from the server to replace the outer HTML of the button.

```fsharp
let handleClick : HttpHandler =
    let html =
        _h2' "Hello, World from the Server!"

    Response.ofHtml html
```

And lastly, we'll make Falco aware of these routes by adding them to the `endpoints` list.

```fsharp
let endpoints =
    [
        get "/" handleIndex
        get "/click" handleClick
    ]
```

Save the file and run the application:

```shell
> dotnet run
```

Navigate to `https://localhost:5001` in your browser and click the button. You should see the text "Hello, World from the Server!" appear in place of the button.

## htmx Attributes

### `hx-[get|post|put|patch|delete]`

```fsharp
_button [ Hx.put "/messages" ] [
    _text "Put to Messages" ]
```

### `hx-trigger`

```fsharp
_div [ Hx.post "/mouse-enter"; Hx.trigger "mouseenter" ] [
    _text "Here mouse, mouse!" ]

// Trigger modifiers
_div [ Hx.post "/mouse-enter"; Hx.trigger ("mouseenter", [HxTrigger.Once]) ] [
    _text "Here mouse, mouse!" ]

// Trigger filters
_div [ Hx.post "/mouse-enter"; Hx.trigger ("mouseenter", [HxTrigger.Once], "ctrlKey") ] [
    _text "Here mouse, mouse!" ]
```

### `hx-target`

```fsharp
_form [] [
    _input [ Hx.get "/search"; Hx.target "#search-results" ]
]
_div [ _id_ "search-results" ] []
```

### `hx-swap`

```fsharp
_button [ Hx.post "/like"; Hx.swapOuterHtml ] [
    _text "Like" ]
```

### `hx-swap-oob`

```fsharp
_div [ _id_ "message"; Hx.swapOobOn ] [
    _text "Swap me directly" ]

// Equivalent to:
_div [ _id_ "message"; Hx.swapOob HxSwap.OuterHTML ] [
    _text "Swap me directly" ]

// With a selector:
_div [ _id_ "message"; Hx.swapOob (HxSwap.InnerHTML, "#falco") ] [
    _text "Swap me directly" ]
```

### `hx-select`

```fsharp
_button [ Hx.get "/info"; Hx.select "#info-detail"; Hx.swapOuterHtml ] [
    _text "Get Info" ]
```

### `hx-select-oob`

```fsharp
_div [ _id_ "alert" ] []
_button [ Hx.get "/info"; Hx.select "#info-detail"; Hx.swapOuterHtml; Hx.selectOob "#alert" ] [
    _text "Get Info" ]
```

### `hx-boost`

```fsharp
_div [ Hx.boostOn ] [
    _a [ _href_ "/blog" ] [ _text "Blog" ] ]
```

### `hx-push-url`

```fsharp
_div [ Hx.get "/account"; Hx.pushUrl true ] [
    _text "Go to My Account" ]

// Or short form:
_div [ Hx.get "/account"; Hx.pushUrlOn ] [
    _text "Go to My Account" ]

// Or specify URL:
_div [ Hx.get "/account"; Hx.pushUrl "/my-account" ] [
    _text "Go to My Account" ]
```

### `hx-sync`

```fsharp
_form [ Hx.post "/store" ] [
    _input [ _name_ "title"; Hx.post "/validate"; Hx.trigger "change"; Hx.sync ("form", HxSync.Abort) ] ]
```

### `hx-include`

```fsharp
_button [ Hx.post "/register"; Hx.includeCss "[name=email]" ] [
    _text "Register!" ]
_span [] [
    _text "Enter email: "
    _input [ _name_ "email"; _type_ "email" ] [] ]

// Hx.includeCss "[name=email]" is equivalent to:
_button [ Hx.post "/register"; Hx.include' (HxTarget.Css "[name=email]") ] [
    _text "Register!" ]
_span [] [
    _text "Enter email: "
    _input [ _name_ "email"; _type_ "email" ] [] ]
```

### `hx-params`

```fsharp
_div [ Hx.get "/example"; Hx.params "*" ] [
    _text "Get Some HTML, Including Params" ]
```

### `hx-vals`

```fsharp
_div [ Hx.get "/example"; Hx.vals """{"myVal": "My Value"}""" ] [
    _text "Get Some HTML, Including A Value in the Request" ]

// Or with a dynamic value:
_div [ Hx.get "/example"; Hx.vals "js:{myVal: calculateValue()}" ] [
    _text "Get Some HTML, Including a Dynamic Value from Javascript in the Request" ]
```

### `hx-confirm`

```fsharp
_button [ Hx.delete "/account"; Hx.confirm "Are you sure you wish to delete your account?" ] [
    _text "Delete My Account" ]
```

### `hx-disable`

```fsharp
_div [ Hx.disable ] []
```

### `hx-disabled-elt`

```fsharp
_button [ Hx.post "/example"; Hx.disabledThis ] [
    _text "Post It!" ]

// Equivalent to:
_button [ Hx.post "/example"; Hx.disabled HxTarget.This ] [
    _text "Post It!" ]
```

### `hx-inherit`

```fsharp
_div [ Hx.targerCss "#tab-container"; Hx.inherit' "hx-target" ] [
    _a [ Hx.boostOn; _href_ "/tab1" ] [ _text "Tab 1" ]
    _a [ Hx.boostOn; _href_ "/tab2" ] [ _text "Tab 2" ]
    _a [ Hx.boostOn; _href_ "/tab3" ] [ _text "Tab 3" ] ]
```

### `hx-disinherit`

```fsharp
_div [ Hx.boostOn; Hx.select "#content"; Hx.targetCss "#content"; Hx.disinherit "hx-target" ] [
    _button [ Hx.get "/test" ] [] ]
```

### `hx-encoding`

```fsharp
_form [ Hx.encodingMultipart ] [
    (* ... form controls ... *) ]
```

### `hx-ext`

```fsharp
_div [ Hx.ext "example" ] [
    _text "Example extension is used in this part of the tree..."
    _div [ Hx.ext "ignore:example" ] [
        _text "... but it will not be used in this part." ] ]
```

### `hx-headers`

```fsharp
_div [ Hx.get "/example"; Hx.headers [ "myHeader", "My Value" ] ] [
    _text "Get Some HTML, Including A Custom Header in the Request" ]

// Or to evaluate a dynamic value:
_div [ Hx.get "/example"; Hx.headers ([ "myHeader", "calculateValue()" ], true) ] [
    _text "Get Some HTML, Including A Custom Header in the Request" ]
// ^-- produces hx-headers='js:{"myHeader": calculateValue()}'
```

### `hx-history`

```fsharp
_div [ Hx.historyOff ] []
```

### `hx-history-elt`

```fsharp
_div [ Hx.historyElt ] []
```

### `hx-indicator`

```fsharp
_div [] [
    _button [ Hx.post "/example"; Hx.indicator "#spinner" ] [
        _text "Post It!" ]
    _img [ _id_ "spinner"; _class_ "htmx-indicator"; _src_ "/img/bars.svg" ] ]
```

## htmx Response Modifiers

### `HX-Location`

```fsharp
Response.withHxLocation "/new-location"
>> Response.ofHtml (_h1' "HX-Location")

// this is equivalent to:
Response.withHxLocationOptions ("/new-location", None)
>> Response.ofHtml (_h1' "HX-Location")

// with context:
let ctx = HxLocation(event = "click", source = HxTarget.This)
Response.withHxLocationOptions ("/new-location", Some ctx)
>> Response.ofHtml (_h1' "HX-Location")
```

### `HX-Push-Url`

```fsharp
Response.withHxPushUrl "/new-push-url"
>> Response.ofHtml (_h1' "HX-Push-Url")
```

### `HX-Redirect`

```fsharp
Response.withHxRedirect "/redirect-url"
>> Response.ofHtml (_h1' "HX-Redirect")
```

### `HX-Refresh`

```fsharp
Response.withHxRefresh
>> Response.ofHtml (_h1' "HX-Refresh")
```

### `HX-Replace-Url`

```fsharp
Response.withHxReplaceUrl "/replace-url"
>> Response.ofHtml (_h1' "HX-Replace-Url")
```

### `HX-Reswap`

```fsharp
Response.withHxReswap HxSwap.InnerHTML
>> Response.ofHtml (_h1' "HX-Reswap")

// with selector:
Response.withHxReswap (HxSwap.OuterHTML, "#falco")
>> Response.ofHtml (_h1' "HX-Reswap")
```

### `HX-Retarget`

```fsharp
Response.withHxRetarget HxTarget.This
>> Response.ofHtml (_h1' "HX-Retarget")

// with selector:
Response.withHxRetarget (HxTarget.Css "#falco")
>> Response.ofHtml (_h1' "HX-Retarget")
```

### `HX-Trigger`

```fsharp
Response.withHxTrigger (HxTriggerResponse.Events [ "myEvent" ])
>> Response.ofHtml (_h1' "HX-Trigger")

// or with detailed events (content is serialized to JSON):
Response.withHxTrigger (HxTriggerResponse.DetailedEvents [ ("myEvent", {| someData = 123 |}) ])
>> Response.ofHtml (_h1' "HX-Trigger")
```

### `HX-Trigger-After-Settle`

```fsharp
Response.withHxTrigger (HxTriggerResponse.Events [ "myEvent" ])
>> Response.withHxTriggerAfterSettle
>> Response.ofHtml (_h1' "HX-Trigger-After-Settle")
```

### `HX-Trigger-After-Swap`

```fsharp
Response.withHxTrigger (HxTriggerResponse.Events [ "myEvent" ])
>> Response.withHxTriggerAfterSwap
>> Response.ofHtml (_h1' "HX-Trigger-After-Swap")
```

### `HX-Reselect`

```fsharp
Response.withHxReselect "#falco"
>> Response.ofHtml (_h1' "HX-Reselect")
```

## Template Fragments

Falco.Htmx has built-in support for [template fragments](https://htmx.org/docs/#template-fragments). This allows you to return only a fragment of a larger HTML document in response to an htmx request, without having to create separate template function for each fragment.

This is supported by the `Response.ofFragment` function, the `hx-swap` attribute and optionally the `hx-select` attribute.

For an overview, see the [Click & Load](/examples/ClickToLoad/) example.

## Kudos

Big thanks and kudos to [@dpraimeyuu](https://github.com/dpraimeyuu) for their collaboration in starting this repo!

## Find a bug?

There's an [issue](https://github.com/FalcoFramework/Falco.Htmx/issues) for that.

## License

Licensed under [Apache License 2.0](https://github.com/FalcoFramework/Falco.Htmx/blob/master/LICENSE).
