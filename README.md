# vawi-cs-sl

This is a repository for the VAWi Course C Sharp for Software Engineers.

## How to use this repository
The repository is structured in the following way:
bin/ - Contains the compiled binaries of the project (mac)
obj/ - Contains the object files of the project (mac)
src/ - Contains the source files of the project. The main file is Program.cs.
data/ - Contains the data storage files of the project. When the program is run, the data is loaded from this folder. Example data is provided.

## How to compile the project
This project uses the .NET Core SDK version 7.0. To compile the project, you need to install the .NET Core SDK. You can download it from [here](https://dotnet.microsoft.com/download).

The used and recommended IDE is Visual Studio Code. You can download it from [here](https://code.visualstudio.com/).

The .vscode folder contains a tasks.json file which contains the build task. To compile the project, press Ctrl+Shift+B (Windows) or Cmd+Shift+B (Mac).

## How to run the project
To run the project you can either use the .NET Core CLI or Visual Studio Code.
The .vscode folder contains a launch.json file which contains the run task. To run the project, press F5.
The bin/ folder contains already the compiled binaries for arm macs. You can just execute the vawi-cs-sl file from the root folder.