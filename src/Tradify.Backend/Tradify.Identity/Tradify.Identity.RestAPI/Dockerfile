FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tradify.Identity.RestAPI/Tradify.Identity.RestAPI.csproj", "Tradify.Identity.RestAPI/"]
RUN dotnet restore "Tradify.Identity.RestAPI/Tradify.Identity.RestAPI.csproj"
COPY . .
WORKDIR "/src/Tradify.Identity.RestAPI"
RUN dotnet build "Tradify.Identity.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tradify.Identity.RestAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tradify.Identity.RestAPI.dll"]
