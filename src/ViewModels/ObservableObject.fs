namespace FSharpWPF.ViewModels
open System.ComponentModel

    type ObservableObject() =
        let propertyChangedEvent = new Event<_,_>()
        interface INotifyPropertyChanged with
            [<CLIEvent>]
            member this.PropertyChanged = propertyChangedEvent.Publish
        
        member this.SetProperty<'T>(property:ref<'T>, value:'T, name:string) =
            property.Value <- value
            propertyChangedEvent.Trigger(this, PropertyChangedEventArgs(name))