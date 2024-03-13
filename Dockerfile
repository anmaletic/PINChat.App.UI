FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# install python
RUN apt-get update && \
    apt-get install -y python3 && \
    rm -rf /var/lib/apt/lists/*
ENV PATH="/usr/bin/python3:${PATH}"

COPY ["PINChat.App.Blazor/PINChat.App.Blazor.csproj", "PINChat.App.Blazor/"]

RUN dotnet workload install wasm-tools
RUN dotnet restore "PINChat.App.Blazor/PINChat.App.Blazor.csproj"
COPY . .
WORKDIR "/src/PINChat.App.Blazor"
RUN dotnet build "PINChat.App.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PINChat.App.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Stage 2: Serve the application with Nginx
FROM nginx:alpine

# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

# Copy the built Blazor WebAssembly application into Nginx's html directory
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
