# Game Library & Player Stats Manager

This repository contains an early scaffold for a C# console coursework project built for .NET 10.

## Current Structure

- `src/GameLibraryManager`
  - `Program.cs` for the application entry point
  - `Core` for menu and application flow classes
  - `Models` for data classes
  - `Services` for future business logic
  - `Utilities` for helper methods
- `tests/GameLibraryManager.Tests` for unit tests
- `docs` for design documents and screenshots later
- `GameLibraryManager.sln` as the solution file

## Current Status

This step only includes a simple startup message, a placeholder main menu, and a starter test project.
Business logic has not been implemented yet.

## Suggested Commands

```bash
dotnet restore
dotnet build
dotnet run --project src/GameLibraryManager
dotnet test
```

## Quick Run on Windows

You can also double-click:

- `run-project.bat`

Or run in PowerShell:

```powershell
.\run-project.ps1
```
