@echo off
REM DeusXMachinaCommand Installer Build Script
echo ========================================
echo DeusXMachinaCommand Installer Builder
echo ========================================
echo.

REM Check if Inno Setup is installed
set INNO_PATH=C:\Program Files (x86)\Inno Setup 6\ISCC.exe
if not exist "%INNO_PATH%" (
    echo ERROR: Inno Setup not found at: %INNO_PATH%
    echo.
    echo Please install Inno Setup from: https://jrsoftware.org/isinfo.php
    echo Or update the INNO_PATH variable in this script if installed elsewhere.
    echo.
    pause
    exit /b 1
)

REM Check if Release build exists
if not exist "DeusXMachinaCommand\bin\Release\DeusXMachinaCommand.dll" (
    echo WARNING: Release build not found!
    echo Please build the project in Release configuration first.
    echo.
    pause
    exit /b 1
)

REM Create output directory if it doesn't exist
if not exist "DeusXMachinaCommand\installer_output" (
    mkdir "DeusXMachinaCommand\installer_output"
    echo Created installer_output directory
)

echo Building installer...
echo.

REM Build the installer
"%INNO_PATH%" "DeusXMachinaCommand_Setup.iss"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo SUCCESS! Installer built successfully.
    echo ========================================
    echo.
    echo Output location: DeusXMachinaCommand\installer_output\
    echo.
    
    REM Open the output folder
    explorer "DeusXMachinaCommand\installer_output"
) else (
    echo.
    echo ========================================
    echo ERROR: Build failed!
    echo ========================================
    echo.
    echo Check the error messages above.
)

echo.
pause

