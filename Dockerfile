# syntax=docker/dockerfile:1
# Imagem da API (JVM-like: SDK para build, runtime enxuto do ASP.NET).
# A orquestracao Aspire (AppHost) roda em desenvolvimento; veja o README.

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/Catalog.Api/Catalog.Api.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
