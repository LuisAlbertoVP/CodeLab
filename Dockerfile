# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar toda la solución de una sola vez
COPY . ./

# Restaurar dependencias y publicar la API
RUN dotnet restore CodeLab.Api.Web/CodeLab.Api.Web.csproj
RUN dotnet publish CodeLab.Api.Web/CodeLab.Api.Web.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "CodeLab.Api.Web.dll"]
