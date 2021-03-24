open System
open System.Windows
open FSharpWPF.ViewModels

// Define a function to construct a message to print

[<STAThread>]
[<EntryPoint>]
let main argv =        
    let application = 
        Uri("/FSharpwPF;component/App.xaml", UriKind.Relative)
        |> fun uri -> Application.LoadComponent(uri) :?> Application

    application.Run() |> ignore

    0 // return an integer exit code