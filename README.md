# Dry

A simple project scaffolding tool rewritten in F#.

## Usage

You can run the tool using `dotnet run`:

```bash
# Create a JavaScript project (default)
dotnet run --project src/Dry.fsproj -- init MyProject

# Create an F# project
dotnet run --project src/Dry.fsproj -- init MyFSharpProject -t fsharp

# Create a Python project
dotnet run --project src/Dry.fsproj -- init MyPythonProject --type python
```

## Supported Templates

- `javascript` (Node.js basic)
- `fsharp` (Console App)
- `python` (Basic script)

## Development

Requirements: .NET SDK 8.0 or later.

Build:
```bash
dotnet build
```


<!-- Rewrite v2 -->
