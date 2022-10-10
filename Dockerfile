FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . .
WORKDIR /app/src
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY Authors ./Authors
COPY Posts ./Posts
COPY Settings ./Settings
COPY src/wwwroot ./src/wwwroot
COPY --from=build-env /app/src/out ./src/out

EXPOSE 80
WORKDIR /app/src
ENTRYPOINT ["dotnet", "out/deferat.dll"]
