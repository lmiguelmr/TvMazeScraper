#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TvMazeScraper.Api/TvMazeScraper.Api.csproj", "src/TvMazeScraper.Api/"]
COPY ["src/TvMazeScraper.Application/TvMazeScraper.Application.csproj", "src/TvMazeScraper.Application/"]
COPY ["src/TvMazeScraper.Domain/TvMazeScraper.Domain.csproj", "src/TvMazeScraper.Domain/"]
COPY ["src/TvMazeScraper.Infrastructure/TvMazeScraper.Infrastructure.csproj", "src/TvMazeScraper.Infrastructure/"]
RUN dotnet restore "./TvMazeScraper.Api/./TvMazeScraper.Api.csproj"
COPY . .
WORKDIR "/src/src/TvMazeScraper.Api"
RUN dotnet build "./TvMazeScraper.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TvMazeScraper.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TvMazeScraper.Api.dll"]