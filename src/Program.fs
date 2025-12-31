open System
open System.IO
open Dry.Core

let rec copyDirectory sourceDir destDir projectName =
    if not (Directory.Exists(destDir)) then
        Directory.CreateDirectory(destDir) |> ignore

    // Files
    for file in Directory.GetFiles(sourceDir) do
        let fileName = Path.GetFileName(file)
        let newFileName = replaceFilename fileName projectName
        let destFile = Path.Combine(destDir, newFileName)

        // Read, replace, write
        let content = File.ReadAllText(file)
        let newContent = replaceContent content projectName
        File.WriteAllText(destFile, newContent)
        printfn "Created: %s" destFile

    // Sub-directories
    for subdir in Directory.GetDirectories(sourceDir) do
        let dirName = Path.GetFileName(subdir)
        let newDirName = replaceFilename dirName projectName
        let destSubDir = Path.Combine(destDir, newDirName)
        copyDirectory subdir destSubDir projectName

[<EntryPoint>]
let main argv =
    let options = parseArgs (argv |> Array.toList) defaultOptions

    match options.Command, options.ProjectName with
    | Some "init", Some name ->
        if not (isValidProjectName name) then
            printfn "Error: '%s' is not a valid project name." name
            1
        else
            let templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", options.ProjectType)
            let targetPath = Path.Combine(Environment.CurrentDirectory, name)

            if not (Directory.Exists(templatePath)) then
                printfn "Error: Template '%s' not found." options.ProjectType
                let templatesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates")
                if Directory.Exists(templatesDir) then
                    printfn "Available templates: %s" (String.Join(", ", Directory.GetDirectories(templatesDir) |> Array.map Path.GetFileName))
                else
                    printfn "Templates directory not found."
                1
            elif Directory.Exists(targetPath) then
                printfn "Error: Directory '%s' already exists." name
                1
            else
                printfn "Initializing project '%s' using template '%s'..." name options.ProjectType
                try
                    copyDirectory templatePath targetPath name
                    printfn "Done!"
                    0
                with
                | ex ->
                    printfn "An error occurred: %s" ex.Message
                    1
    | _ ->
        printfn "Usage: dry init <project-name> [-t <type>]"
        printfn "Types: javascript (default), fsharp, python"
        1
