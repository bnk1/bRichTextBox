@echo off
setlocal

REM Folder of this script = solution folder
set SOLUTION_DIR=%~dp0

REM Path to the project file (adjust if different)
set PROJECT=%SOLUTION_DIR%BRichTextBox.csproj

REM Ensure API key is available via environment variable
if "%NUGET_API_KEY%"=="" (
    echo ERROR: NUGET_API_KEY environment variable is not set.
    echo Go to System Properties -> Environment Variables and add it.
    pause
    exit /b 1
)

echo === Building and packing BRichTextBox (Release) ===
dotnet pack "%PROJECT%" -c Release
if errorlevel 1 goto :error

REM Extract <Version>...</Version> from the csproj
for /f "tokens=3 delims=<>" %%v in ('findstr /i "<Version>" "%PROJECT%"') do set PKG_VERSION=%%v

if "%PKG_VERSION%"=="" (
    echo ERROR: Could not extract Version from BRichTextBox.csproj
    pause
    exit /b 1
)

set NUPKG_PATH=%SOLUTION_DIR%bin\Release\BRichTextBox.%PKG_VERSION%.nupkg

if not exist "%NUPKG_PATH%" (
    echo ERROR: Package not found: %NUPKG_PATH%
    echo Expected path: %NUPKG_PATH%
    pause
    exit /b 1
)

echo === Pushing BRichTextBox.%PKG_VERSION%.nupkg to nuget.org ===
dotnet nuget push "%NUPKG_PATH%" --api-key "%NUGET_API_KEY%" --source https://api.nuget.org/v3/index.json
if errorlevel 1 goto :error

set SNUPKG_PATH=%SOLUTION_DIR%bin\Release\BRichTextBox.%PKG_VERSION%.snupkg

if exist "%SNUPKG_PATH%" (
    echo === Pushing symbols BRichTextBox.%PKG_VERSION%.snupkg ===
    dotnet nuget push "%SNUPKG_PATH%" --api-key "%NUGET_API_KEY%" --source https://api.nuget.org/v3/index.json
    if errorlevel 1 (
        echo WARNING: Symbols push failed ^(symbols server may be busy^). Package published successfully.
        echo To retry: dotnet nuget push "%SNUPKG_PATH%" --api-key %%NUGET_API_KEY%% --source https://api.nuget.org/v3/index.json
    )
)

echo === Publish completed successfully (version %PKG_VERSION%) ===

echo === Tagging release v%PKG_VERSION% ===
set SOLUTION_DIR_NOTRIM=%SOLUTION_DIR:~0,-1%
git -C "%SOLUTION_DIR_NOTRIM%" tag v%PKG_VERSION%
git -C "%SOLUTION_DIR_NOTRIM%" push origin v%PKG_VERSION%

pause
exit /b 0

:error
echo === Publish FAILED ===
exit /b 1
