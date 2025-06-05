# Falco.Htmx

[![NuGet Version](https://img.shields.io/nuget/v/Falco.Htmx.svg)](https://www.nuget.org/packages/Falco.Htmx)
[![build](https://github.com/FalcoFramework/Falco.Htmx/actions/workflows/build.yml/badge.svg)](https://github.com/FalcoFramework/Falco.Htmx/actions/workflows/build.yml)

```fsharp
open Falco.Markup
open Falco.Htmx

let demo =
    Elem.button
        [ Hx.get "/click-me"
          Hx.swapOuterHtml
          Hx.targetCss "#wrapper" ]
        [ Text.raw "Reset" ]
```

[Falco.Htmx](https://github.com/FalcoFramework/Falco.Htmx) brings type-safe [htmx](https://htmx.org/) support to [Falco](https://github.com/FalcoFramework/Falco). It provides a complete mapping of all attributes, typed request data and ready-made response modifiers.

## Key Features

- Idiomatic mapping of `htmx` attributes (i.e., `hx-get`, `hx-post`, `hx-target` etc.).
- Typed access to htmx request headers.
- Prepared response modifiers for common use-cases (i.e., `HX-location`, `HX-Push-Url`).

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
        Elem.html [] [
            Elem.head [] [
                Elem.script [ Attr.src HtmxScript.cdnSrc ] [] ]
            Elem.body [] [
                Text.h1 "Example: Hello World"
                Elem.button
                    [ Hx.get "/click"
                      Hx.swapOuterHtml ]
                    [ Text.raw "Click Me" ] ] ]

    Response.ofHtml html
```

Next, we'll define a handler for the click event that will return HTML from the server to replace the outer HTML of the button.

```fsharp
let handleClick : HttpHandler =
    let html =
        Text.h2 "Hello, World from the Server!"

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
Elem.button [ Hx.put "/messages" ] [
    Text.raw "Put to Messages" ]
```

### `hx-trigger`

```fsharp
Elem.div [ Hx.post "/mouse-enter"; Hx.trigger "mouseenter" ] [
    Text.raw "Here mouse, mouse!" ]

// Trigger modifiers
Elem.div [ Hx.post "/mouse-enter"; Hx.trigger ("mouseenter", [HxTrigger.Once]) ] [
    Text.raw "Here mouse, mouse!" ]

// Trigger filters
Elem.div [ Hx.post "/mouse-enter"; Hx.trigger ("mouseenter", [HxTrigger.Once], "ctrlKey") ] [
    Text.raw "Here mouse, mouse!" ]
```

### `hx-target`

```fsharp
Elem.form [] [
    Elem.input [ Hx.get "/search"; Hx.target "#search-results" ]
]
Elem.div [ Attr.id "search-results" ] []
```

### `hx-swap`

```fsharp
Elem.button [ Hx.post "/like"; Hx.swapOuterHtml ] [
    Text.raw "Like" ]
```

### `hx-swap-oob`

```fsharp
Elem.div [ Attr.id "message"; Hx.swapOobOn ] [
    Text.raw "Swap me directly" ]

// Equivalent to:
Elem.div [ Attr.id "message"; Hx.swapOob HxSwap.OuterHTML ] [
    Text.raw "Swap me directly" ]

// With a selector:
Elem.div [ Attr.id "message"; Hx.swapOob (HxSwap.InnerHTML, "#falco") ] [
    Text.raw "Swap me directly" ]
```

### `hx-select`

```fsharp
Elem.button [ Hx.get "/info"; Hx.select "#info-detail"; Hx.swapOuterHtml ] [
    Text.raw "Get Info" ]
```

### `hx-select-oob`

```fsharp
Elem.div [ Attr.id "alert" ] []
Elem.button [ Hx.get "/info"; Hx.select "#info-detail"; Hx.swapOuterHtml; Hx.selectOob "#alert" ] [
    Text.raw "Get Info" ]
```

### `hx-boost`

```fsharp
Elem.div [ Hx.boostOn ] [
    Elem.a [ Attr.href "/blog" ] [ Text.raw "Blog" ] ]
```

### `hx-push-url`

```fsharp
Elem.div [ Hx.get "/account"; Hx.pushUrl true ] [
    Text.raw "Go to My Account" ]

// Or short form:
Elem.div [ Hx.get "/account"; Hx.pushUrlOn ] [
    Text.raw "Go to My Account" ]

// Or specify URL:
Elem.div [ Hx.get "/account"; Hx.pushUrl "/my-account" ] [
    Text.raw "Go to My Account" ]
```

### `hx-sync`

```fsharp
Elem.form [ Hx.post "/store" ] [
    Elem.input [ Attr.name "title"; Hx.post "/validate"; Hx.trigger "change"; Hx.sync ("form", HxSync.Abort) ] ]
```

### `hx-include`

```fsharp
Elem.button [ Hx.post "/register"; Hx.includeCss "[name=email]" ] [
    Text.raw "Register!" ]
Elem.span [] [
    Text.raw "Enter email: "
    Elem.input [ Attr.name "email"; Attr.type' "email" ] [] ]

// Hx.includeCss "[name=email]" is equivalent to:
Elem.button [ Hx.post "/register"; Hx.include' (HxTarget.Css "[name=email]") ] [
    Text.raw "Register!" ]
Elem.span [] [
    Text.raw "Enter email: "
    Elem.input [ Attr.name "email"; Attr.type' "email" ] [] ]
```

### `hx-params`

```fsharp
Elem.div [ Hx.get "/example"; Hx.params "*" ] [
    Text.raw "Get Some HTML, Including Params" ]
```

### `hx-vals`

```fsharp
Elem.div [ Hx.get "/example"; Hx.vals """{"myVal": "My Value"}""" ] [
    Text.raw "Get Some HTML, Including A Value in the Request" ]

// Or with a dynamic value:
Elem.div [ Hx.get "/example"; Hx.vals "js:{myVal: calculateValue()}" ] [
    Text.raw "Get Some HTML, Including a Dynamic Value from Javascript in the Request" ]
```

### `hx-confirm`

```fsharp
Elem.button [ Hx.delete "/account"; Hx.confirm "Are you sure you wish to delete your account?" ] [
    Text.raw "Delete My Account" ]
```

### `hx-disable`

```fsharp
Elem.div [ Hx.disable ] []
```

### `hx-disabled-elt`

```fsharp
Elem.button [ Hx.post "/example"; Hx.disabledThis ] [
    Text.raw "Post It!" ]

// Equivalent to:
Elem.button [ Hx.post "/example"; Hx.disabled HxTarget.This ] [
    Text.raw "Post It!" ]
```

### `hx-inherit`

```fsharp
Elem.div [ Hx.targerCss "#tab-container"; Hx.inherit' "hx-target" ] [
    Elem.a [ Hx.boostOn; Attr.href "/tab1" ] [ Text.raw "Tab 1" ]
    Elem.a [ Hx.boostOn; Attr.href "/tab2" ] [ Text.raw "Tab 2" ]
    Elem.a [ Hx.boostOn; Attr.href "/tab3" ] [ Text.raw "Tab 3" ] ]
```

### `hx-disinherit`

```fsharp
Elem.div [ Hx.boostOn; Hx.select "#content"; Hx.targetCss "#content"; Hx.disinherit "hx-target" ] [
    Elem.button [ Hx.get "/test" ] [] ]
```

### `hx-encoding`

```fsharp
Elem.form [ Hx.encodingMultipart ] [
    (* ... form controls ... *) ]
```

### `hx-ext`

```fsharp
Elem.div [ Hx.ext "example" ] [
    Text.raw "Example extension is used in this part of the tree..."
    Elem.div [ Hx.ext "ignore:example" ] [
        Text.raw "... but it will not be used in this part." ] ]
```

### `hx-headers`

```fsharp
Elem.div [ Hx.get "/example"; Hx.headers [ "myHeader", "My Value" ] ] [
    Text.raw "Get Some HTML, Including A Custom Header in the Request" ]

// Or to evaluate a dynamic value:
Elem.div [ Hx.get "/example"; Hx.headers ([ "myHeader", "calculateValue()" ], true) ] [
    Text.raw "Get Some HTML, Including A Custom Header in the Request" ]
// ^-- produces hx-headers='js:{"myHeader": calculateValue()}'
```

### `hx-history`

```fsharp
Elem.div [ Hx.historyOff ] []
```

### `hx-history-elt`

```fsharp
Elem.div [ Hx.historyElt ] []
```

### `hx-indicator`

```fsharp
Elem.div [] [
    Elem.button [ Hx.post "/example"; Hx.indicator "#spinner" ] [
        Text.raw "Post It!" ]
    Elem.img [ Attr.id "spinner"; Attr.class' "htmx-indicator"; Attr.src "/img/bars.svg" ] ]
```

## Kudos

Big thanks and kudos to [@dpraimeyuu](https://github.com/dpraimeyuu) for their collaboration in starting this repo!

## Find a bug?

There's an [issue](https://github.com/FalcoFramework/Falco.Htmx/issues) for that.

## License

Licensed under [Apache License 2.0](https://github.com/FalcoFramework/Falco.Htmx/blob/master/LICENSE).
