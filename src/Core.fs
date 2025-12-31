namespace Dry

open System
open System.IO

module Core =

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

    let isValidProjectName (name: string) =
        let invalids = Path.GetInvalidFileNameChars()
        not (String.IsNullOrWhiteSpace(name)) && name.IndexOfAny(invalids) < 0
