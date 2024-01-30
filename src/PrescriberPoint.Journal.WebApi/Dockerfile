FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PrescriberPoint.Journal.WebApi/PrescriberPoint.Journal.WebApi.csproj", "src/PrescriberPoint.Journal.WebApi/"]
COPY ["src/PrescriberPoint.Journal.Application/PrescriberPoint.Journal.Application.csproj", "src/PrescriberPoint.Journal.Application/"]
COPY ["src/PrescriberPoint.Journal.Domain/PrescriberPoint.Journal.Domain.csproj", "src/PrescriberPoint.Journal.Domain/"]
COPY ["src/PrescriberPoint.Journal.Persistence/PrescriberPoint.Journal.Persistence.csproj", "src/PrescriberPoint.Journal.Persistence/"]
COPY ["src/PrescriberPoint.Journal.Persistence.Pgsql/PrescriberPoint.Journal.Persistence.Pgsql.csproj", "src/PrescriberPoint.Journal.Persistence.Pgsql/"]
RUN dotnet restore "src/PrescriberPoint.Journal.WebApi/PrescriberPoint.Journal.WebApi.csproj"
COPY . .
WORKDIR "/src/src/PrescriberPoint.Journal.WebApi"
RUN dotnet build "PrescriberPoint.Journal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PrescriberPoint.Journal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrescriberPoint.Journal.WebApi.dll"]
