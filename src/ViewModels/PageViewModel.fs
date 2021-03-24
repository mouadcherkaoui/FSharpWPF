namespace FSharpWPF.ViewModels
open System
open System.Collections.ObjectModel
open System.Windows
open System.Windows.Controls

    type PageViewModel() =
        inherit ObservableObject()
        let mutable title: string = ""
        let mutable regions = ObservableCollection<UserControl>()

        member this.Title 
            with get() = title
            and set(value) = this.SetProperty(ref title, value, nameof(this.Title))
        
        member this.Regions
            with get() = regions
            and set(value) = this.SetProperty(ref regions, value, nameof(this.Regions)) 