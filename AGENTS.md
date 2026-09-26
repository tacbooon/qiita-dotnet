# AGENT.md

This repository provides a .NET client library for Qiita API v2.

## Technology Stack

- Language: C# 14
- Target Framework: `net10.0`
- Document: DocFX
- Tests: TUnit (Planned)
- External Dependencies: None

## Commands

- Setup: `dotnet tool restore`
- Build: `dotnet build`
- Document Build: `dotnet docfx docs/docfx.json`

## Code Style

- Write XML comments for all public APIs in Japanese.
- Write code comments in Japanese.
- Write artifact strings (logs, exceptions, CLI output) in English.
- Use `System.Text.Json` source generation for AOT and trimming support.
  - Register public models in `QiitaJsonSerializerContext`.
  - Register internal DTOs in `QiitaInternalJsonSerializerContext`.

## Workflows

- Respond in Japanese.
- Do not commit or push unless asked.
- Write commit messages in Japanese.

## Sandbox

- The agent runs on `anthropics/sandbox-runtime` when the environment variable `SANDBOX_RUNTIME` is set.
- In the sandbox:
  - File, network, and Unix-domain socket access may be denied.
  - `docfx` is unavailable.
- Report the exact blocked target to the user for approval when access is denied.

## REST API Specifications

Fetch only the needed section on demand because both below are large.

- [HTML](https://qiita.com/api/v2/docs): Auth, scopes, rate limits, pagination, errors, and examples.
- [JSON Hyper-Schema](https://qiita.com/api/v2/schema): Exact field types, required params, and paths.
