@echo off
REM ClassicPanel Build Script (Windows)
REM Builds ClassicPanel with Debug or Release configuration
REM Usage: build.bat [Debug|Release]
REM   Debug: Build in Debug configuration (default)
REM   Release: Build in Release configuration

setlocal enabledelayedexpansion

REM Default to Release if no argument provided
set CONFIG=Release

REM Parse arguments
if "%1"=="" goto :config_set
if /i "%1"=="Debug" set CONFIG=Debug
if /i "%1"=="Release" set CONFIG=Release

:config_set
echo ========================================
echo ClassicPanel Build Script
echo ========================================
echo Configuration: %CONFIG%
echo ========================================
echo.

REM Check if .NET SDK is available
dotnet --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK not found. Please install .NET 10 SDK.
    exit /b 1
)

REM Change to source directory
cd /d "%~dp0..\src"
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Could not change to src directory.
    exit /b 1
)

REM Clean previous build (optional - uncomment if needed)
REM echo Cleaning previous build...
REM dotnet clean -c %CONFIG% -p:GenerateAssemblyInfo=false
REM if %ERRORLEVEL% NEQ 0 (
REM     echo WARNING: Clean failed, continuing with build...
REM )

REM Build the project
echo Building ClassicPanel (%CONFIG% configuration)...
echo.
dotnet build -c %CONFIG% -p:GenerateAssemblyInfo=false
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ========================================
    echo BUILD FAILED
    echo ========================================
    exit /b 1
)

echo.
echo ========================================
echo BUILD SUCCEEDED
echo ========================================
echo.

REM Determine output path (relative to src directory)
if /i "%CONFIG%"=="Debug" (
    set OUTPUT_PATH=..\build\debug\ClassicPanel.exe
) else (
    set OUTPUT_PATH=..\build\release\ClassicPanel.exe
)

REM Convert to absolute path for checking
for %%F in ("%OUTPUT_PATH%") do set OUTPUT_ABS=%%~fF
if exist "!OUTPUT_ABS!" (
    echo Output: !OUTPUT_ABS!
    echo.
) else (
    echo WARNING: Expected output file not found at !OUTPUT_ABS!
    echo.
)

:end
echo ========================================
echo Build Complete
echo ========================================
echo.
echo To run the application:
if /i "%CONFIG%"=="Debug" (
    echo   Debug:   ..\build\debug\ClassicPanel.exe
) else (
    echo   Release: ..\build\release\ClassicPanel.exe
)
echo.
exit /b 0

