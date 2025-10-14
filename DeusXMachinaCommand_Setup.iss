; Inno Setup Script for DeusXMachinaCommand
; Tecnomatix eMPower Plugin Installer

#define MyAppName "DeusXMachinaCommand"
#define MyAppVersion "1.0.0.0"
#define MyAppPublisher "SenseFlow, Inc."
#define MyAppURL "https://deusxmachina.dev/"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
AppId={{fc134844-e79d-47d6-a688-c254824e3567}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={code:GetDefaultInstallDir}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
; Allow user to select installation directory
UsePreviousAppDir=no
OutputDir=DeusXMachinaCommand\installer_output
OutputBaseFilename=DeusXMachinaCommand_Setup_{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64
UninstallDisplayName={#MyAppName} for Tecnomatix
UninstallDisplayIcon={app}\DotNetCommands\DeusXMachinaCommand.dll
UninstallFilesDir={app}\DeusXMachinaCommand
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppName} Installer for Tecnomatix eMPower

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Main plugin DLL (required)
Source: "DeusXMachinaCommand\bin\Release\DeusXMachinaCommand.dll"; DestDir: "{app}\DotNetCommands"; Flags: ignoreversion
; NOTE: PDB files are not included in production installer (only needed for development/debugging)
; NOTE: Tecnomatix.Engineering.Olp.dll is not included as it should already exist in Tecnomatix installation
; NOTE: Don't use "Flags: ignoreversion" on any shared system Files

; XML registration of the plugin for process simulate applications
Source: "installer_resources\DeusXMachinaCommand.xml"; DestDir: "{app}\DotNetExternalApplications"; Flags: ignoreversion 

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Code]
function GetDefaultInstallDir(Param: String): String;
var
  RegistryPath: String;
begin
  // Try to get path from registry (check eMPower key)
  if RegQueryStringValue(HKLM, 'SOFTWARE\Tecnomatix\eMPower', 'eMPowerDir', RegistryPath) then
  begin
    // Remove trailing backslash if present
    if Copy(RegistryPath, Length(RegistryPath), 1) = '\' then
      RegistryPath := Copy(RegistryPath, 1, Length(RegistryPath) - 1);
    
    Result := RegistryPath;
  end
  else
  begin
    // Fallback default if registry not found
    Result := ExpandConstant('{autopf}\Tecnomatix\eMPower');
  end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  InstallDir: String;
begin
  Result := True;
  
  // Validate on the directory selection page
  if CurPageID = wpSelectDir then
  begin
    InstallDir := WizardDirValue;
    
    // Check if directory exists or can be created
    if not DirExists(InstallDir + '\DotNetCommands') then
    begin
      MsgBox('The installation path does not appear to be valid:' + #13#10 +
              InstallDir + #13#10#13#10 +
              'Please ensure you select a valid Process Simulate installation directory' + #13#10 +
              'that contains the DotNetCommands folder (or where it can be created).' + #13#10#13#10 +
              'Example: C:\Program Files\Tecnomatix_2502\eMPower', 
              mbError, MB_OK);
      Result := False;
      Exit;
    end;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    MsgBox('DeusXMachinaCommand has been successfully installed!' + #13#10#13#10 +
           'The plugin has been installed to:' + #13#10 +
           ExpandConstant('{app}') + #13#10#13#10 +
           'Please restart Tecnomatix eMPower to load the new plugin.', 
           mbInformation, MB_OK);
  end;
end;

[UninstallDelete]
Type: files; Name: "{app}\DotNetCommands\DeusXMachinaCommand.dll"
Type: files; Name: "{app}\DotNetExternalApplications\DeusXMachinaCommand.xml"
