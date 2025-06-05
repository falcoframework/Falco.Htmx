# Changelog

All notable changes to this project will be documented in this file.

## [1.0.1]

### Fixed

- HTML encoding [issue](https://github.com/falcoframework/Falco.Htmx/issues/8) `System.Net.WebUtility.HtmlEncode` was applied to `Hx.vals`, `Hx.headers`, `Hx.request`

## [1.0.0]

### Added

- Integration of HTMX into Falco.Markup.
- Automated mapping of HTMX request headers for easy consumption.
- `HttpResponseModifier` functions for HTMX response headers.
