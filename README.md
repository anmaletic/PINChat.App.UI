# PINChat
- #### https://pinchat.anmal.dev/
## Summary:
- WebChat application using .NET Technology
- Hosted on private Kubernetes server using Argo CICD
- Using GitHub Actions to build Docker Image, tag it with new version and notify ArgoApps
  repository to update hosted application to latest version

### Projects:
1. PINChat.Api
   - https://github.com/anmaletic/PINChat.Api
1. PINChat.ChatServer
   - https://github.com/anmaletic/PINChat.ChatServer
2. PINChat.App -  *Not used anymore, replaced with PINChat.App.UI*
   - https://github.com/anmaletic/PINChat.App
3. PINChat.App.UI
   - https://github.com/anmaletic/PINChat.App.UI
4. PINChat.Data
   - https://github.com/anmaletic/PINChat.Data
   - https://dbdocs.io/antonio.maletic/PINChat?view=relationships

### Project board:
- https://github.com/users/anmaletic/projects/6


## 1. PINChat.Api
- ##### Tech stack:
	- ASP.NET Core Web App (MVC) with Individual Identification
	- EntityFramework for connection with Authentication Database
	- Dapper for connection with Application Database
- ##### Summary:
	- Web application used for accessing database

## 2. PINChat.ChatServer
- ##### Tech stack:
	- ASP.NET Core Web Api
- ##### Summary:
	- Web application that uses SignalR for hosting ChatHub

## 3. PINChat.App - Not used anymore
- ##### Tech stack:
	- ASP Hosted Blazor WebAssembly App
	- On server side used SignalR for ChatHub
- ##### Summary:
	- On client side progressive application used to connect to database through PINChat.Api
	  hosted on another location and to connect to ChatHub hosted on server side of application

## 4. PINChat.App.UI
- ##### Tech stack:
	- Web: Blazor WebAssembly App
	- Mobile: MAUI (WIP)
	- Desktop: Avalonia (WIP) 
- ##### Summary:
	- Blazor WASM
		- Web frontend used to login and save messages to database using PINChat.Api and to exchange messages using PINChat.ChatServer	
   
## 5. PINChat.Data
- ##### Tech stack:
	- Visual Studio SQL project
	- MS SQL Server
- ##### Summary:
	- Project with all the tables and stored procedures for publishing to SQL Server
