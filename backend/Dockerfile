# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia todo o conteúdo do projeto
COPY . .

# Restaura pacotes e publica
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia a build do stage anterior
COPY --from=build /app/out .

# Porta exposta (ajuste conforme necessário)
EXPOSE 5000

# Executa a API
ENTRYPOINT ["dotnet", "IntegradorApi.dll"]
