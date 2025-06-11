FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

# Restore pe soluție (care include BookSaw.Api)
RUN dotnet restore

# Publicăm doar proiectul principal
RUN dotnet publish "src/BookSaw.Api/BookSaw.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookSaw.Api.dll"]
