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

> TODO

## Kudos

Big thanks and kudos to [@dpraimeyuu](https://github.com/dpraimeyuu) for their collaboration in starting this repo!

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Falco.Htmx/issues) for that.

## License

Built with ♥ by [Pim Brouwers](https://github.com/pimbrouwers) and [Damian Plaza](https://github.com/dpraimeyuu). Licensed under [Apache License 2.0](https://github.com/pimbrouwers/Falco.Htmx/blob/master/LICENSE).
