# simon-on-the-streets

[![Build status](https://ci.appveyor.com/api/projects/status/8c6292quvgyimxdt?svg=true)](https://ci.appveyor.com/project/ChrisDobby/simon-on-the-streets)

## Requirements

- [Mono](http://www.mono-project.com/) on MacOS/Linux
- [.NET Framework 4.6.2](https://support.microsoft.com/en-us/help/3151800/the--net-framework-4-6-2-offline-installer-for-windows) on Windows
- [node.js](https://nodejs.org/) - JavaScript runtime
- [yarn](https://yarnpkg.com/) - Package manager for npm modules
- [dotnet SDK 2.1.4](https://github.com/dotnet/cli/releases/tag/v2.1.4) The .NET Core SDK
- Other tools like [Paket](https://fsprojects.github.io/Paket/) or [FAKE](https://fake.build/) will also be installed by the build script.

## Development mode

This development stack is designed to be used with minimal tooling. An instance of Visual Studio Code together with the excellent [Ionide](http://ionide.io/) plugin should be enough.

Start the development mode with:

    > build.cmd run // on windows
    $ ./build.sh run // on unix
