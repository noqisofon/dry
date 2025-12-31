open System
open System.IO

type Options = {
    Command: string option
    ProjectName: string option
    ProjectType: string
}

let defaultOptions = {
    Command = None
    ProjectName = None
    ProjectType = "javascript"
}

let rec parseArgs (args: string list) options =
    match args with
    | [] -> options
    | "init" :: name :: rest when not (name.StartsWith("-")) ->
        parseArgs rest { options with Command = Some "init"; ProjectName = Some name }
    | "-t" :: t :: rest | "--type" :: t :: rest ->
        parseArgs rest { options with ProjectType = t }
    | _ :: rest -> parseArgs rest options

let replaceContent (content: string) (projectName: string) =
    content.Replace("{{ProjectName}}", projectName)

let replaceFilename (filename: string) (projectName: string) =
    filename.Replace("__PROJECT_NAME__", projectName)

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
        let templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", options.ProjectType)
        let targetPath = Path.Combine(Environment.CurrentDirectory, name)

        if not (Directory.Exists(templatePath)) then
            printfn "Error: Template '%s' not found." options.ProjectType
            printfn "Available templates: %s" (String.Join(", ", Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates")) |> Array.map Path.GetFileName))
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
        printfn "Usage: Dry init <project-name> [-t <type>]"
        printfn "Types: javascript (default), fsharp, python"
        1
