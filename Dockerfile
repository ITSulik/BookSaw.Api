FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR /tools
COPY . .

WORKDIR /src
RUN dotnet restore

WORKDIR /tools
RUN dotnet restore

COPY ["src", "src"]
COPY ["tools", "tools"]
RUN dotnet publish "src/BookSaw.Api/BookSaw.Api.csproj" -c Release -o /app/publish/api

RUN dotnet publish "tools/BookSaw.Seeder/BookSaw.Seeder.csproj" -c Release -o /app/publish/tools

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish/api .
COPY --from=build /app/publish/tools .
ENTRYPOINT ["dotnet", "BookSaw.Api.dll"]
