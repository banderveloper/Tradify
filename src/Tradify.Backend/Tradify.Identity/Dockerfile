FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tradify.Identity.Domain/Tradify.Identity.Domain.csproj", "Tradify.Identity.Domain/Tradify.Identity.Domain.csproj"]
COPY ["Tradify.Identity.Application/Tradify.Identity.Application.csproj", "Tradify.Identity.Application/Tradify.Identity.Application.csproj"]
COPY ["Tradify.Identity.Persistence/Tradify.Identity.Persistence.csproj", "Tradify.Identity.Persistence/Tradify.Identity.Persistence.csproj"]
COPY ["Tradify.Identity.RestAPI/Tradify.Identity.RestAPI.csproj", "Tradify.Identity.RestAPI/Tradify.Identity.RestAPI.csproj"]

COPY . .
WORKDIR "/src/Tradify.Identity.RestAPI"
RUN dotnet build "Tradify.Identity.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tradify.Identity.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tradify.Identity.RestAPI.dll"]