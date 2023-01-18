FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IsbmClientExample/IsbmClientExample.csproj", "src/IsbmClientExample/"]
RUN dotnet restore "src/IsbmClientExample/IsbmClientExample.csproj"
COPY . .
WORKDIR "/src/src/IsbmClientExample"
RUN dotnet build "IsbmClientExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IsbmClientExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
LABEL org.opencontainers.image.source https://github.com/tmr-imd/Isbm2RestClient
WORKDIR /app
COPY --from=publish --chown=appuser /app/publish .
RUN find /app -type d -exec chmod 750 {} + ; \
    find /app -type f -exec chmod 640 {} + ;
ENTRYPOINT ["dotnet", "IsbmClientExample.dll"]
