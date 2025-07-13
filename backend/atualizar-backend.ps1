# Script para atualizar a API Integrador no Docker (Windows PowerShell)

Write-Host "🔧 Publicando projeto..."
dotnet publish ./IntegradorApi/IntegradorApi.csproj -c Release -o ./IntegradorApi/bin/Release/net9.0

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Falha na publicação do projeto. Abortando..."
    exit 1
}

Write-Host "🐳 Recriando imagem Docker..."
docker build -t integrador-api .

Write-Host "⛔ Parando container antigo (se existir)..."
docker stop integrador-api 2>$null
docker rm integrador-api 2>$null

Write-Host "🚀 Subindo container atualizado..."
docker run -d --name integrador-api `
  --network integrador-net `
  -p 5000:8080 `
  integrador-api

Write-Host "✅ Container atualizado e em execução: http://localhost:5000"


