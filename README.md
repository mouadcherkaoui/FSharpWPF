# FSharpWPF

a demo WPF application in F# language ready to use as starter template.

this will be a reference about making a Wpf application using .Net 5.0 and F# since there is no vailable template for this kind of project.

the idea here is to understand some F# basics to show how we can leverage the multi paradigm aspect, in one side we will use MVVM architectural pattern to manage our presentation logic, in the other side we will use functional approach to fill some business requirements.

## project Setup

this section is more about how to make a WPF project with F#, since actually there is no out of the box available template, where the plumbing part is and what is needed to make the project build and run.

first we will take a look to the project file itself, simply you create an f# console application and change it to desktop application by changing the project sdk for the dotnet core 3.1 or just changing the target for the .net 5 one, assuming you already have vscode and the dotnet core or .Net 5 sdk installed, you can create new console application using dotnet cli and use the -lang switch to specify fsharp language, now open a powershell command prompt in your projects folder type these command `dotnet new console -lang f# -o FSharpMVVM && code ./FSharpMVVM`.
this will create a new console project in the folder FSharpMVVM and will start vscode in that folder, click on the "FSharpMVVM.fsproj" in the editor you should see something like  "without the commented section" :

```xml
<Project Sdk="Microsoft.NET.Sdk">
<!--Project Sdk="Microsoft.NET.Sdk.WindowsDesktop"-->
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <!--UseWPF>true</UseWPF>-->
  </PropertyGroup>
</Project>
```

or for .Net 5:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>
</Project>
```

for dotnet core you need to change the Sdk attribute value and add the "UseWPF" tag like bellow:

```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop"> 
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
</Project>
```

and for .Net 5 you need to add -Windows to the target framework like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0-Windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
</Project>
```
now we need to add new App.xaml file which will define our application root object, you can grab an existing one or copy the xaml bellow:

```xml
<Application xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="Views/MainWindow.xaml">
</Application>
```

as you see we set the StartupUri  to the view "Views\MainWindow.xaml", which we'll add too:

```xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:vm="clr-namespace:FSharpMVVM.ViewModels;assembly=FSharpMVVM"
    Title="MainWindow" Height="450" Width="800">
    <Window.DataContext>
        <vm:MainWindowViewModel/>
    </Window.DataContext>
    <Grid>

    </Grid>
</Window>
```

now we need to add our MainWindowViewModel which we declared in our view as an xml namespace :

```xml
xmlns:vm="clr-namespace:FSharpMVVM.ViewModels;assembly=FSharpMVVM"
    Title="MainWindow" Height="450" Width="800">
```

and in used in our "DataContext":

```xml
    <Window.DataContext>
        <vm:MainWindowViewModel/>
    </Window.DataContext>
```

first our ViewModel should implement INotifyPropertyChanged interface to be able to notify WPF binding mechanism about changes that occurs whether in the property or the UI:

```fsharp
namespace FSharpMVVM.ViewModels
open System.Windows
open System.ComponentModel

    type MainWindowViewModel() =
        let mutable title = ""
        let mutable content = ""
        let eventPublisher = new Event<_,_>()
        interface INotifyPropertyChanged with
            [<CLIEvent>]
            member this.PropertyChanged = eventPublisher.Publish
        member this.Title 
            with get() = value
            and set(value) = 
                title <- value
                this.eventPublisher.Trigger(new PropertyChangedEventArgs("Title"))
        member this.Content 
            with get() = value
            and set(value) = 
                content <- value
                this.eventPublisher.Trigger(new PropertyChangedEventArgs("Content"))
```

we'll return back to disect this view model and explain its definition and why we are using th "Event<_,_>" object.

now we can go to the "Program.fs" file and update it to start our application:

```fsharp
// we add our namespace opens/imports equivalent in csharp
open System
open System.Windows
open FSharpWPF.ViewModels

// Define a function to construct a message to print

[<STAThread>]
[<EntryPoint>]
let main argv =    
    (* we define a Uri object that we pass through the pipe operator '|>' to our Application.LoadComponent static method 
    and we cast it "using :?> operator" to an Application type
    *)

    let application = 
        Uri("/FSharpwPF;component/App.xaml", UriKind.Relative)
        |> fun uri -> Application.LoadComponent(uri) :?> Application

    application.Run() |> ignore

    0 // return an integer exit code
```

now check your project file "FSharpMVVM.fsproj" and make sure that the "App.xaml", and "MainWindow.xaml" are declared as resources, and the "Program.fs" is the last one at the bottom in the `<ItemGroup>` section:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net5.0-Windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>    
    <Compile Include="ViewModels\MainWindowViewModel.fs" />    
    <Compile Include="Program.fs" />
  </ItemGroup>
  
  <ItemGroup>    
    <Resource Include="Views\MainWindow.xaml"/>
    <Resource Include="App.xaml"/>    
  </ItemGroup>
</Project>

```

now we are ready to run our great blank app!

```bash
dotnet run
```

now we can continue adding views, viewmodels and functions and more, all with the great abilities that a functional first/multiparadigm language can offer.
