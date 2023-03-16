FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get -y update \
    && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs \
    && apt-get clean \
    && echo 'node verions:' $(node -v) \
    && echo 'npm version:' $(npm -v)

WORKDIR /src
COPY ["BlazorServerExample/BlazorServerExample.csproj", "BlazorServerExample/BlazorServerExample.csproj"]
RUN dotnet restore --use-current-runtime "BlazorServerExample/BlazorServerExample.csproj"

COPY . .
WORKDIR "/src/BlazorServerExample"
RUN dotnet build "BlazorServerExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorServerExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
LABEL org.opencontainers.image.source https://github.com/tmr-imd/Isbm2RestClient
WORKDIR /app
COPY --from=publish --chown=appuser /app/publish .
RUN rm /app/appsettings.json && mv /app/appsettings.Release.json /app/appsettings.json
RUN find /app -type d -exec chmod 750 {} + ; \
    find /app -type f -exec chmod 640 {} + ;
EXPOSE 80
ENTRYPOINT ["dotnet", "BlazorServerExample.dll"]
