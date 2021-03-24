namespace FSharpWPF.ViewModels
open System.Collections.ObjectModel    
open System.Windows
open ViewModels

    type MainWindowViewModel() = 
        inherit ObservableObject()
        let mutable title = ""
        let mutable content = "" 
        
        member this.Title
            with get() = title 
            and set(value) = 
                title <- value
                this.SetProperty (ref title, value, nameof(this.Title))

        member this.Content
            with get() = content
            and set(value) = this.SetProperty(ref content, value, nameof(this.Content))

        member this.Items = new ObservableCollection<obj>()
        
        member this.ButtonContent = "Click!"
        
        member this.ShowMessageCommand = 
            RelayCommand((fun f -> MessageBox.Show(this.Title) |> ignore), (fun f -> true))

        