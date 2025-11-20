@echo off
setlocal

REM Folder of this script = solution folder
set SOLUTION_DIR=f:\Code\BRichTextBox\

REM Path to the project file (adjust if different)
set PROJECT=%SOLUTION_DIR%BRichTextBox\BRichTextBox.csproj

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

set NUPKG_PATH=%SOLUTION_DIR%BRichTextBox\bin\Release\BRichTextBox.%PKG_VERSION%.nupkg

if not exist "%NUPKG_PATH%" (
    echo ERROR: Package not found: %NUPKG_PATH%
    echo Expected path: %NUPKG_PATH%
    pause
    exit /b 1
)

echo === Pushing BRichTextBox.%PKG_VERSION%.nupkg to nuget.org ===
dotnet nuget push "%NUPKG_PATH%" --api-key "%NUGET_API_KEY%" --source https://api.nuget.org/v3/index.json
if errorlevel 1 goto :error

echo === Publish completed successfully (version %PKG_VERSION%) ===
pause
exit /b 0

:error
echo === Publish FAILED ===
pause
exit /b 1
