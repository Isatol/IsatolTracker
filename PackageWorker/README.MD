# Package Worker Service

Background Service to track packages.

## Description

This service runs every 20 minutes to monitor packages. Use the TrackerDAL project to access the database, which is located in the **DEMO_DB.sql** file in the Database folder

## Install

This service is installed as Windows Service with a PowerShell command.
If you do not want to use it as Windows Service and you want to switch to Web App, edit the following in the PackageWorker.csproj file:

- Remove **RuntimeIdentifiers** line
- Remove **IsTransformWebConfigDisabled** line

If you want, also remove the Nuget package:
**Microsoft.Extensions.Hosting.WindowsServices**

In **Program.cs** remove .UseWindowsService() line

To install as Windows Service use this command in PowerShell as Administrator

```cmd
New-Service -Name {SERVICE-NAME} -BinaryPathName "{EXE-PATH}" -Description "{SERVICE-DESCRIPTION}" -DisplayName "{DISPLAY-NAME}" -StartupType Automatic
```

- {SERVICE-NAME}: The name of the service, for example TestService
- {EXE-PATH}: The PackageWorker.exe path
- {SERVICE-DESCRIPTION}: A short description, for example My Windows Service Test.
- {DISPLAY-NAME}: The name to locate the service in the Windows Services list

### Remove Windows Service

Execute this command in PowerSheell as Administrator

```cmd
sc.exe delete {SERVICE-NAME}
```
