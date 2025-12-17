@echo off
REM ClassicPanel Build Script (Windows)
REM Builds ClassicPanel with Debug or Release configuration
REM Usage: build.bat [Debug|Release] [--publish]
REM   Debug: Build in Debug configuration (default)
REM   Release: Build in Release configuration
REM   --publish: Also publish self-contained executable after successful build

setlocal enabledelayedexpansion

REM Default to Release if no argument provided
set CONFIG=Release
set PUBLISH=false

REM Parse arguments
if "%1"=="" goto :config_set
if /i "%1"=="Debug" set CONFIG=Debug
if /i "%1"=="Release" set CONFIG=Release
if "%1"=="--publish" set PUBLISH=true
if "%2"=="--publish" set PUBLISH=true
if /i "%2"=="Debug" set CONFIG=Debug
if /i "%2"=="Release" set CONFIG=Release

:config_set
echo ========================================
echo ClassicPanel Build Script
echo ========================================
echo Configuration: %CONFIG%
echo Publish: %PUBLISH%
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
    set OUTPUT_PATH=..\build\debug\net10.0-windows\win-x64\ClassicPanel.exe
) else (
    set OUTPUT_PATH=..\build\release\net10.0-windows\win-x64\ClassicPanel.exe
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

REM Publish if requested or if Release build
if "%PUBLISH%"=="true" goto :publish
if /i "%CONFIG%"=="Release" goto :publish
goto :end

:publish
echo ========================================
echo Publishing self-contained executable...
echo ========================================
echo.
dotnet publish -c %CONFIG% ^
    -p:PublishSingleFile=true ^
    -p:PublishReadyToRun=true ^
    -p:SelfContained=true ^
    -p:RuntimeIdentifier=win-x64 ^
    -p:GenerateAssemblyInfo=false

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ========================================
    echo PUBLISH FAILED
    echo ========================================
    exit /b 1
)

echo.
echo ========================================
echo PUBLISH SUCCEEDED
echo ========================================
echo.
set PUBLISH_PATH=..\build\publish\ClassicPanel.exe
REM Convert to absolute path for checking
for %%F in ("%PUBLISH_PATH%") do set PUBLISH_ABS=%%~fF
REM Small delay to ensure file system has updated
timeout /t 1 /nobreak >nul 2>&1
if exist "!PUBLISH_ABS!" (
    echo Published executable: !PUBLISH_ABS!
    echo.
    REM Get file size
    for %%A in ("!PUBLISH_ABS!") do set SIZE=%%~zA
    set /a SIZE_MB=!SIZE! / 1048576
    echo Size: !SIZE_MB! MB (self-contained with ReadyToRun)
    echo.
) else (
    REM Try checking again after another brief delay
    timeout /t 1 /nobreak >nul 2>&1
    if exist "!PUBLISH_ABS!" (
        echo Published executable: !PUBLISH_ABS!
        echo.
        for %%A in ("!PUBLISH_ABS!") do set SIZE=%%~zA
        set /a SIZE_MB=!SIZE! / 1048576
        echo Size: !SIZE_MB! MB (self-contained with ReadyToRun)
        echo.
    ) else (
        echo WARNING: Published executable not found at !PUBLISH_ABS!
        echo Note: File may still be writing. Check manually if needed.
        echo.
    )
)

:end
echo ========================================
echo Build Complete
echo ========================================
echo.
echo To run the application:
if /i "%CONFIG%"=="Debug" (
    echo   Debug:   ..\build\debug\net10.0-windows\win-x64\ClassicPanel.exe
) else (
    echo   Release: ..\build\release\net10.0-windows\win-x64\ClassicPanel.exe
)
if "%PUBLISH%"=="true" (
    echo   Published: ..\build\publish\ClassicPanel.exe
) else if /i "%CONFIG%"=="Release" (
    echo   Published: ..\build\publish\ClassicPanel.exe
)
echo.
exit /b 0

