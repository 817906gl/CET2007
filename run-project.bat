@echo off
setlocal

cd /d "%~dp0"

echo Restoring packages...
dotnet restore
if errorlevel 1 goto :fail

echo.
echo Building solution...
dotnet build
if errorlevel 1 goto :fail

echo.
echo Running tests...
dotnet test --no-build
if errorlevel 1 goto :fail

echo.
echo Starting console app...
dotnet run --project .\src\GameLibraryManager
goto :end

:fail
echo.
echo Command failed. Review the messages above.
pause
exit /b 1

:end
echo.
pause
