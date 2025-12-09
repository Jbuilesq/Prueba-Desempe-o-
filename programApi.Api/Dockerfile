# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080

# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["programApi.Api/programApi.Api.csproj", "programApi.Api/"]
COPY ["programApi.Domain/programApi.Domain.csproj", "programApi.Domain/"]
COPY ["programApi.Application/programApi.Application.csproj", "programApi.Application/"]
COPY ["programApi.Infrastructure/programApi.Infrastructure.csproj", "programApi.Infrastructure/"]
RUN dotnet restore "programApi.Api/programApi.Api.csproj"
COPY . .
WORKDIR "/src/programApi.Api"
RUN dotnet build "./programApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------- Publish ----------
FROM build AS publish
RUN dotnet publish "./programApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------- Final ----------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Variables de entorno (opcional: pueden venir de docker-compose)
ENV DB_HOST=${DB_HOST:-localhost} \
    DB_PORT=${DB_PORT:-3306} \
    DB_NAME=${DB_NAME:-talentoplus} \
    DB_USER=${DB_USER:-root} \
    DB_PASS=${DB_PASS:-root} \
    JWT_SECRET=${JWT_SECRET:-talentoPlusSuperSecretKeyAtLeast32BytesLong!} \
    JWT_ISSUER=${JWT_ISSUER:-talentoplus} \
    JWT_AUDIENCE=${JWT_AUDIENCE:-talentoplusemployees} \
    JWT_EXPIRY_MINUTES=${JWT_EXPIRY_MINUTES:-60} \
    JWT_REFRESH_DAYS=${JWT_REFRESH_DAYS:-7}

ENTRYPOINT ["dotnet", "programApi.Api.dll"]
