# DeusXMachinaCommand Installer Setup Guide

This project uses Inno Setup to create a Windows installer for the DeusXMachinaCommand Tecnomatix plugin.

## Prerequisites

1. **Download and Install Inno Setup**
   - Download from: https://jrsoftware.org/isinfo.php
   - Recommended: Inno Setup 6.x or later
   - Install with default options

## Building the Installer

### Method 1: Using Inno Setup Compiler GUI

1. Build your project in **Release** configuration in Visual Studio
2. Open `DeusXMachinaCommand_Setup.iss` with Inno Setup Compiler
3. Click **Build** menu → **Compile** (or press Ctrl+F9)
4. The installer will be created in: `DeusXMachinaCommand\installer_output\DeusXMachinaCommand_Setup_1.0.0.0.exe`

### Method 2: Using Command Line

```cmd
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" DeusXMachinaCommand_Setup.iss
```

## Customizing the Installer

You can modify the following in `DeusXMachinaCommand_Setup.iss`:

### Company Information
```pascal
#define MyAppPublisher "Your Company Name"
#define MyAppURL "https://www.yourcompany.com/"
```

### Version Number
The version is currently set to match `AssemblyInfo.cs` (1.0.0.0). Update both files when releasing new versions:
- `DeusXMachinaCommand\Properties\AssemblyInfo.cs`
- `DeusXMachinaCommand_Setup.iss` (line 6)

### Installation Path
The installer **requires** Tecnomatix to be installed. It detects the installation from registry:
```
HKLM\SOFTWARE\Siemens\Tecnomatix\InstallPath
```
If Tecnomatix is not found, the installer will exit with an error message.

### Including Debug Symbols (Development Only)
By default, PDB files are excluded. If you need them for development/debugging, add this line to the `[Files]` section:
```pascal
Source: "DeusXMachinaCommand\bin\Release\DeusXMachinaCommand.pdb"; DestDir: "{app}"; Flags: ignoreversion
```

## What Gets Installed

The installer packages and installs:
- `DeusXMachinaCommand.dll` - Main plugin library

**Note:** 
- PDB debug symbols are NOT included in production installers (only needed during development)
- The installer does NOT include `Tecnomatix.Engineering.Olp.dll` as it should already exist in the Tecnomatix installation

## Installation Process

1. Run `DeusXMachinaCommand_Setup_1.0.0.0.exe`
2. The installer validates that Tecnomatix is installed
3. If Tecnomatix is not found, the installer exits with an error message
4. Files are copied to: `{Tecnomatix}\eMPower\DotNetCommands\`
5. Users must restart Tecnomatix eMPower to load the new plugin

## Uninstallation

Users can uninstall via:
- Windows Settings → Apps → DeusXMachinaCommand
- Or from the Start Menu → DeusXMachinaCommand → Uninstall

## Troubleshooting

### "Tecnomatix installation not found"
If you receive this error:
- Ensure Siemens Tecnomatix is installed on your system
- Verify the registry key exists: `HKLM\SOFTWARE\Siemens\Tecnomatix`
- Install Tecnomatix before attempting to install this plugin
- If Tecnomatix is installed but not detected, verify it was installed correctly and the registry entry exists

### "Cannot open file" errors during build
- Ensure Visual Studio is not running
- Make sure the Release build completed successfully
- Check that files exist in `DeusXMachinaCommand\bin\Release\`

## Automating Builds

### Pre-build: Update Version
You can automate version updates by modifying both:
1. `AssemblyInfo.cs`
2. `DeusXMachinaCommand_Setup.iss`

### Post-build Event (Optional)
Add to your `.csproj` file to auto-build installer after Release builds:

```xml
<Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition="'$(Configuration)' == 'Release'">
  <Exec Command="&quot;C:\Program Files (x86)\Inno Setup 6\ISCC.exe&quot; &quot;$(SolutionDir)DeusXMachinaCommand_Setup.iss&quot;" 
        ContinueOnError="true" />
</Target>
```

## Distribution

The final installer (`DeusXMachinaCommand_Setup_1.0.0.0.exe`) is a single executable that can be:
- Distributed to end users
- Uploaded to a download server
- Included in software packages
- Sent via email (if size permits)

## Security Note

The installer is **not digitally signed** by default. For production releases, consider:
- Obtaining a code signing certificate
- Using SignTool to sign the installer
- This prevents Windows SmartScreen warnings

