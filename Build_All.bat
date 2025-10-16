@echo off
setlocal

echo ========================================
echo DeusXMachinaCommand - Full Build (Release + Installer)
echo ========================================
echo.

REM Change to script directory
cd /d "%~dp0"

REM 1) Locate MSBuild
set MSBUILD_EXE=
if exist "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_EXE=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD_EXE set "MSBUILD_EXE=MSBuild.exe"

if not defined MSBUILD_EXE (
  echo ERROR: Could not find MSBuild.exe. Please install Visual Studio Build Tools.
  echo Download: https://visualstudio.microsoft.com/downloads/
  exit /b 1
)
echo Using MSBuild: %MSBUILD_EXE%
echo.

REM 2) Build solution in Release
set "SLN=DeusXMachinaCommand.sln"
if not exist "%SLN%" (
  echo ERROR: Solution not found at %CD%\%SLN%
  exit /b 1
)

REM Extract version from git tag first (needed for MSBuild)
for /f "tokens=*" %%i in ('git describe --tags --abbrev=0 2^>nul') do set GIT_TAG=%%i
if not defined GIT_TAG (
  echo WARNING: No git tag found, using default version
  set GIT_TAG=0.1.1
) else (
  REM Remove 'v' prefix if present (e.g., v0.1.1 -> 0.1.1)
  set GIT_TAG=%GIT_TAG:v=%
)
echo Using version: %GIT_TAG%
echo.

echo Building solution in Release...
"%MSBUILD_EXE%" "%SLN%" /t:Rebuild /p:Configuration=Release /p:Version=%GIT_TAG% /m /v:minimal
if errorlevel 1 (
  echo.
  echo ERROR: MSBuild failed.
  exit /b 1
)

REM Verify DLL output
set "DLL_PATH=DeusXMachinaCommand\bin\Release\DeusXMachinaCommand.dll"
if not exist "%DLL_PATH%" (
  echo ERROR: Release DLL not found at %DLL_PATH%
  exit /b 1
)
echo Release build OK.
echo.

REM 3) Build installer with Inno Setup
set INNO_EXE=
if exist "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" set "INNO_EXE=C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
if not defined INNO_EXE if exist "C:\Program Files\Inno Setup 6\ISCC.exe" set "INNO_EXE=C:\Program Files\Inno Setup 6\ISCC.exe"

if not defined INNO_EXE (
  echo ERROR: Inno Setup Compiler ISCC.exe not found.
  echo Download: https://jrsoftware.org/isinfo.php
  exit /b 1
)
echo Using Inno Setup: %INNO_EXE%
echo.

set "OUTPUT_DIR=DeusXMachinaCommand\installer_output"
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

echo Building installer...
"%INNO_EXE%" "DeusXMachinaCommand_Setup.iss" /DMyAppVersion=%GIT_TAG%
if errorlevel 1 (
  echo.
  echo ERROR: Installer build failed.
  exit /b 1
)

echo.
echo ========================================
echo SUCCESS: Release and Installer built.
echo Output: %OUTPUT_DIR%
echo ========================================
echo.
start "" "%OUTPUT_DIR%"

endlocal
exit /b 0
