FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /app/src
COPY ["src/BookSaw.Api/BookSaw.Api.csproj", "BookSaw.Api/"]

WORKDIR /app/tools
COPY ["tools/BookSaw.Seeder/BookSaw.Seeder.csproj", "BookSaw.Seeder/"]

WORKDIR /app
RUN dotnet restore "tools/BookSaw.Seeder/BookSaw.Seeder.csproj"
RUN dotnet restore "src/BookSaw.Api/BookSaw.Api.csproj"

COPY ["src", "src"]
COPY ["tools", "tools"]
RUN dotnet publish "src/BookSaw.Api/BookSaw.Api.csproj" -c Release -o /app/publish
RUN dotnet publish "tools/BookSaw.Seeder/BookSaw.Seeder.csproj" -c Release -o /app/publish/tools

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookSaw.Api.dll"]
