$ErrorActionPreference = "Stop"

Set-Location $PSScriptRoot

Write-Host "Restoring packages..."
dotnet restore

Write-Host ""
Write-Host "Building solution..."
dotnet build

Write-Host ""
Write-Host "Running tests..."
dotnet test --no-build

Write-Host ""
Write-Host "Starting console app..."
dotnet run --project .\src\GameLibraryManager
