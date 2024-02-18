FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PINChat.App.Blazor/PINChat.App.Blazor.csproj", "PINChat.App.Blazor/"]
RUN dotnet restore "PINChat.App.Blazor/PINChat.App.Blazor.csproj"
COPY . .
WORKDIR "/src/PINChat.App.Blazor"
RUN dotnet build "PINChat.App.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PINChat.App.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PINChat.App.Blazor.dll"]
