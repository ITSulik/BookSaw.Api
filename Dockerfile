FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "BookSaw.Api.sln"

COPY ["src", "src"]
COPY ["tools", "tools"]
RUN dotnet publish "src/BookSaw.Api/BookSaw.Api.csproj" -c Release -o /app/api

RUN dotnet publish "tools/BookSaw.Seeder/BookSaw.Seeder.csproj" -c Release -o /app/tools

FROM base AS final
WORKDIR /app
COPY --from=build /app/api .
COPY --from=build /app/tools .
ENTRYPOINT ["dotnet", "BookSaw.Api.dll"]
