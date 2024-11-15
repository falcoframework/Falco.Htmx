# Falco.Htmx

[![NuGet Version](https://img.shields.io/nuget/v/Falco.Htmx.svg)](https://www.nuget.org/packages/Falco.Htmx)
[![build](https://github.com/pimbrouwers/Falco.Htmx/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Falco.Htmx/actions/workflows/build.yml)

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

[Falco.Htmx](https://github.com/pimbrouwers/Falco.Htmx) brings type-safe [htmx](https://htmx.org/) support to [Falco](https://github.com/pimbrouwers/Falco). It provides a complete mapping of all attributes, typed request data and ready-made response modifiers.

## Key Features

- Idiomatic mapping of `htmx` attributes (i.e., `hx-get`, `hx-post`, `hx-target` etc.).
- Typed access to htmx request headers.
- Prepared response modifiers for common use-cases (i.e., `HX-location`, `HX-Push-Url`).

## Design Goals

- Create a self-documenting way to integrate htmx into Falco applications.
- Match the specification of htmx as closely as possible, ideally one-to-one.
- Provide type safety without over-abstracting.

## Getting Started

This guide assumes you have a [Falco](https://github.com/pimbrouwers/Falco) project setup. If you don't, you can create a new Falco project using the following command:

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
open Falco.Routing
open Microsoft.AspNetCore.Builder

let bldr = WebApplication.CreateBuilder()
let wapp = bldr.Build()

let endpoints =
    [
    ]

wapp.UseFalco(endpoints)
    .Run()
```

Now, let's incorporate htmx into our Falco application. Update the `Program.fs` file to the following:

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

  <div hx-get="/example" hx-vals='{"myVal": "My Value"}'>Get Some HTML, Including A Value in the Request</div>
  <div hx-get="/example" hx-vals='js:{myVal: calculateValue()}'>Get Some HTML, Including a Dynamic Value from Javascript in the Request</div>


```fsharp
Elem.div [ Hx.get "/example"; Hx.vals """{"myVal": "My Value"}""" ] [
    Text.raw "Get Some HTML, Including A Value in the Request" ]

// Or with a dynamic value:
Elem.div [ Hx.get "/example"; Hx.vals "js:{myVal: calculateValue()}" ] [
    Text.raw "Get Some HTML, Including a Dynamic Value from Javascript in the Request" ]
```

### `hx-confirm`

<button hx-delete="/account" hx-confirm="Are you sure you wish to delete your account?">
  Delete My Account
</button>

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
// TODO
```

### `hx-disinherit`

```fsharp
// TODO
```

### `hx-encoding`

```fsharp
// TODO
```

### `hx-ext`

```fsharp
// TODO
```

### `hx-headers`

```fsharp
// TODO
```

### `hx-history`

```fsharp
// TODO
```

### `hx-history-elt`

```fsharp
// TODO
```

### `hx-indicator`

```fsharp
// TODO
```


## Kudos

Big thanks and kudos to [@dpraimeyuu](https://github.com/dpraimeyuu) for their collaboration in starting this repo!

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Falco.Htmx/issues) for that.

## License

Built with ♥ by [Pim Brouwers](https://github.com/pimbrouwers) and [Damian Plaza](https://github.com/dpraimeyuu). Licensed under [Apache License 2.0](https://github.com/pimbrouwers/Falco.Htmx/blob/master/LICENSE).
