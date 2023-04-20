FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tradify.Chat.Domain/Tradify.Chat.Domain.csproj", "Tradify.Chat.Domain/Tradify.Chat.Domain.csproj"]
COPY ["Tradify.Chat.Application/Tradify.Chat.Application.csproj", "Tradify.Chat.Application/Tradify.Chat.Application.csproj"]
COPY ["Tradify.Chat.Persistence/Tradify.Chat.Persistence.csproj", "Tradify.Chat.Persistence/Tradify.Chat.Persistence.csproj"]
COPY ["Tradify.Chat.RestAPI/Tradify.Chat.RestAPI.csproj", "Tradify.Chat.RestAPI/Tradify.Chat.RestAPI.csproj"]

COPY . .
WORKDIR "/src/Tradify.Chat.RestAPI"
RUN dotnet build "Tradify.Chat.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tradify.Chat.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tradify.Chat.RestAPI.dll"]