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
