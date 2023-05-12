FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ResourceManager.Api/ResourceManager.Api.csproj", "src/ResourceManager.Api/"]
RUN dotnet restore "src/ResourceManager.Api/ResourceManager.Api.csproj"
COPY . .
WORKDIR "/src/src/ResourceManager.Api"
RUN dotnet build "ResourceManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ResourceManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResourceManager.Api.dll"]
