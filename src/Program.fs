open System
open System.Windows
open FSharpWPF.ViewModels

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<STAThread>]
[<EntryPoint>]
let main argv =
    let mainWindow = 
        Uri("/FSharpwPF;component/Views/MainWindow.xaml", UriKind.Relative)
        |> fun uri -> Application.LoadComponent(uri) :?> Window

    mainWindow.ShowDialog() |> ignore
    
    0 // return an integer exit code