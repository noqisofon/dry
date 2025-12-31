module Tests

open System
open Xunit
open Dry.Core

[<Fact>]
let ``parseArgs handles init command correctly`` () =
    let args = ["init"; "MyProject"]
    let options = parseArgs args defaultOptions
    Assert.Equal(Some "init", options.Command)
    Assert.Equal(Some "MyProject", options.ProjectName)
    Assert.Equal("javascript", options.ProjectType)

[<Fact>]
let ``parseArgs handles type flag correctly`` () =
    let args = ["init"; "MyProject"; "-t"; "fsharp"]
    let options = parseArgs args defaultOptions
    Assert.Equal("fsharp", options.ProjectType)

[<Fact>]
let ``replaceContent replaces {{ProjectName}}`` () =
    let content = "Hello {{ProjectName}}"
    let result = replaceContent content "World"
    Assert.Equal("Hello World", result)

[<Fact>]
let ``isValidProjectName rejects invalid characters`` () =
    // Checking platform specific invalid chars might be tricky cross-platform,
    // but standard ones like < > | are usually invalid.
    let invalidChar = System.IO.Path.GetInvalidFileNameChars().[0]
    let name = sprintf "In%cvalid" invalidChar
    Assert.False(isValidProjectName name)
    Assert.True(isValidProjectName "ValidName")
