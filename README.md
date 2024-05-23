# csharp-windows-desktop-maui

An example GUI app for windows, using .NET 8 and the MAUI Framework.

Implements a Dice Roll Game.

## Setting up

- Download and install [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.204-windows-x64-installer)

- Install requirements using a terminal

        dotnet workload install maui

        dotnet tool install -g redth.net.MAUI.check

        maui-check

- Install the .NET MAUI vscode extension.

- Place an image at the following path: 

## Debugging

Press `F5` to run the default launch task. You can also run the program from the console using the following command:

        dotnet run -t:Run -f net8.0-windows10.0.19041.0

## Building

Press `F6` in VSCode to run the default build task.

## Resources

[.NET API](https://learn.microsoft.com/en-us/dotnet/api/)

[Microsoft.Maui.Controls](https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls?view=net-maui-8.0)
