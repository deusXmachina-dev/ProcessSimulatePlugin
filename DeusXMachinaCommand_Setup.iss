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
DefaultDirName={reg:HKLM\SOFTWARE\Siemens\Tecnomatix,InstallPath|{autopf}\Tecnomatix}\eMPower\DotNetCommands
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
function InitializeSetup(): Boolean;
var
  TecnomatixPath: String;
begin
  Result := False;
  
  // Check if Tecnomatix is installed
  if not RegQueryStringValue(HKLM, 'SOFTWARE\Siemens\Tecnomatix', 'InstallPath', TecnomatixPath) then
  begin
    MsgBox('Tecnomatix installation not found!' + #13#10#13#10 +
           'This plugin requires Siemens Tecnomatix to be installed.' + #13#10 +
           'Please install Tecnomatix before installing this plugin.' + #13#10#13#10 +
           'Installation will now exit.', 
           mbError, MB_OK);
    Exit;
  end;
  
  // Verify the path exists
  if not DirExists(TecnomatixPath) then
  begin
    MsgBox('Tecnomatix installation path found in registry, but the directory does not exist:' + #13#10 +
           TecnomatixPath + #13#10#13#10 +
           'Please repair or reinstall Tecnomatix before installing this plugin.' + #13#10#13#10 +
           'Installation will now exit.', 
           mbError, MB_OK);
    Exit;
  end;
  
  // Verify DotNetCommands folder exists
  if not DirExists(TecnomatixPath + '\eMPower\DotNetCommands') then
  begin
    MsgBox('Tecnomatix DotNetCommands folder not found at:' + #13#10 +
           TecnomatixPath + '\eMPower\DotNetCommands' + #13#10#13#10 +
           'Please ensure Tecnomatix eMPower is properly installed.' + #13#10#13#10 +
           'Installation will now exit.', 
           mbError, MB_OK);
    Exit;
  end;
  
  // All checks passed
  Result := True;
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

