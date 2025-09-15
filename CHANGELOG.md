# Changelog

All notable changes to this project will be documented in this file.

## [1.1.0] - 2025-09-15

### Added

- `HX-Location` response header can now be set with or without context via `Response.withHxLocation` and `Response.withHxLocationOptions`.

## [1.0.2] - 2025-09-09

### Added

- HTML fragment support via [Falco 5.0.3](https://github.com/falcoframework/Falco/blob/master/CHANGELOG.md#503---2025-09-09) & [Falco.Markup 1.3.0](https://github.com/falcoframework/Falco.Markup/blob/master/CHANGELOG.md#130-2025-09-09)

## [1.0.1] - 2025-06-05

### Fixed

- HTML encoding [issue](https://github.com/falcoframework/Falco.Htmx/issues/8) `System.Net.WebUtility.HtmlEncode` was applied to `Hx.vals`, `Hx.headers`, `Hx.request`

## [1.0.0] - 2025-01-29

### Added

- Integration of HTMX into Falco.Markup.
- Automated mapping of HTMX request headers for easy consumption.
- `HttpResponseModifier` functions for HTMX response headers.
