module ViewModels
open System
open System.Windows.Input

    type RelayCommand(execute:Action<obj>, canExecute:Func<obj, bool>) = 
        let canExecuteChangedEvent = new Event<_,_>()
        interface ICommand with
            member this.Execute args =
                execute.Invoke(args)
            member this.CanExecute args =  
                canExecute.Invoke(args)
            [<CLIEvent>]
            member this.CanExecuteChanged = canExecuteChangedEvent.Publish