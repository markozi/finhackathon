# finhackathon
[![Open in Dev Containers](https://img.shields.io/static/v1?label=Dev%20Containers&message=Open&color=blue&logo=visualstudiocode)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/markozi/finhackathon)

If you already have VS Code and Docker installed, you can click the badge above or [here](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/markozi/finhackathon) to get started. Clicking these links will cause VS Code to automatically install the Dev Containers extension if needed, clone the source code into a container volume, and spin up a dev container for use.

# Start the example TodoApi
After opening this code repository in a dev container, follow the instruction below.

## Create and trust the developer certificate
inside the dev container terminal, enter the following to create a local dev certificate and trust it.
```
dotnet dev-certs https --trust
```

## Start the example TodoApi
Change into the Project Directory
```
cd TodoApi
```

Start the App
```
dotnet run --launch-profile https
```

Open the following links in a browser to test the web app.
* https://127.0.0.1:7080/swagger/index.html
* https://127.0.0.1:7080/WeatherForecast

Note that you might need to change the port depending on the output of dotnet run. According to https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio-code it is random.

