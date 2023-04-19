FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tradify.Chat.RestAPI/Tradify.Chat.RestAPI.csproj", "Tradify.Chat.RestAPI/"]
RUN dotnet restore "Tradify.Chat.RestAPI/Tradify.Chat.RestAPI.csproj"
COPY . .
WORKDIR "/src/Tradify.Chat.RestAPI"
RUN dotnet build "Tradify.Chat.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tradify.Chat.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tradify.Chat.RestAPI.dll"]
