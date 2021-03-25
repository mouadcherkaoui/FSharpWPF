namespace FSharpWPF.ViewModels
open System.ComponentModel

    type ObservableObject() =
        let propertyChangedEvent = new Event<_,_>()
        interface INotifyPropertyChanged with
            [<CLIEvent>]
            member this.PropertyChanged = propertyChangedEvent.Publish
        
        member this.SetProperty<'T>(property:byref<'T>, value:'T, name:string) =
            property <- value
            propertyChangedEvent.Trigger(this, PropertyChangedEventArgs(name))