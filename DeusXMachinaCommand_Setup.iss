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
DefaultDirName={code:GetInstallPath}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=DeusXMachinaCommand\installer_output
OutputBaseFilename=DeusXMachinaCommand_Setup_{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64
UninstallDisplayName={#MyAppName} for Tecnomatix
UninstallDisplayIcon={app}\DeusXMachinaCommand.dll
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppName} Installer for Tecnomatix eMPower

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Main plugin DLL (required)
Source: "DeusXMachinaCommand\bin\Release\DeusXMachinaCommand.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: PDB files are not included in production installer (only needed for development/debugging)
; NOTE: Tecnomatix.Engineering.Olp.dll is not included as it should already exist in Tecnomatix installation
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Code]
var
  TecnomatixInstallPath: String;

function ValidateTecnomatixPath(Path: String): Boolean;
var
  DotNetCommandsPath: String;
begin
  Result := False;
  
  // Remove trailing backslash if present
  if Copy(Path, Length(Path), 1) = '\' then
    Path := Copy(Path, 1, Length(Path) - 1);
  
  // Check if base path exists
  if not DirExists(Path) then
  begin
    MsgBox('The specified directory does not exist:' + #13#10 +
           Path + #13#10#13#10 +
           'Please verify the path is correct.', 
           mbError, MB_OK);
    Exit;
  end;
  
  // Check for DotNetCommands folder
  DotNetCommandsPath := Path + '\DotNetCommands';
  if not DirExists(DotNetCommandsPath) then
  begin
    MsgBox('This does not appear to be a valid Process Simulate installation path.' + #13#10#13#10 +
           'The following required folder was not found:' + #13#10 +
           DotNetCommandsPath + #13#10#13#10 +
           'Please ensure you specify the root Tecnomatix installation directory' + #13#10 +
           'that contains the DotNetCommands folder structure.', 
           mbError, MB_OK);
    Exit;
  end;
  
  Result := True;
end;

function GetInstallPath(Param: String): String;
begin
  Result := TecnomatixInstallPath + '\DotNetCommands';
end;

function InitializeSetup(): Boolean;
var
  RegistryPath: String;
begin
  Result := False;
  
  // Try to get path from registry first (check eMPower key)
  if RegQueryStringValue(HKLM, 'SOFTWARE\Tecnomatix\eMPower', 'eMPowerDir', RegistryPath) then
  begin
    // Remove trailing backslash if present
    if Copy(RegistryPath, Length(RegistryPath), 1) = '\' then
      RegistryPath := Copy(RegistryPath, 1, Length(RegistryPath) - 1);
    
    if ValidateTecnomatixPath(RegistryPath) then
    begin
      TecnomatixInstallPath := RegistryPath;
      Result := True;
      Exit;
    end;
  end;
  
  // Registry path not found or invalid, show error and exit
  MsgBox('Process Simulate installation not detected in registry.' + #13#10#13#10 +
         'This plugin requires Siemens Tecnomatix Process Simulate to be installed.' + #13#10 +
         'Please install Process Simulate before installing this plugin.' + #13#10#13#10 +
         'Installation will now exit.', 
         mbError, MB_OK);
  Exit;
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
Type: files; Name: "{app}\DeusXMachinaCommand.dll"
