#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RepositoryWithCaching/RepositoryWithCaching.WebApi/RepositoryWithCaching.WebApi.csproj", "RepositoryWithCaching/RepositoryWithCaching.WebApi/"]
COPY ["RepositoryWithCaching/RepositoryWithCaching.Application/RepositoryWithCaching.Application.csproj", "RepositoryWithCaching/RepositoryWithCaching.Application/"]
COPY ["RepositoryWithCaching/RepositoryWithCaching.Domain/RepositoryWithCaching.Domain.csproj", "RepositoryWithCaching/RepositoryWithCaching.Domain/"]
COPY ["RepositoryWithCaching/RepositoryWithCaching.Infrastructure.Shared/RepositoryWithCaching.Infrastructure.Shared.csproj", "RepositoryWithCaching/RepositoryWithCaching.Infrastructure.Shared/"]
COPY ["RepositoryWithCaching/RepositoryWithCaching.Infrastructure.Persistence/RepositoryWithCaching.Infrastructure.Persistence.csproj", "RepositoryWithCaching/RepositoryWithCaching.Infrastructure.Persistence/"]
RUN dotnet restore "RepositoryWithCaching/RepositoryWithCaching.WebApi/RepositoryWithCaching.WebApi.csproj"
COPY . .
WORKDIR "/src/RepositoryWithCaching/RepositoryWithCaching.WebApi"
RUN dotnet build "RepositoryWithCaching.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RepositoryWithCaching.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RepositoryWithCaching.WebApi.dll"]
