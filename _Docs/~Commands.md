# Migration Commands
Add-Migration -Context "EngineDataContext" -Name "Initial" -Project ResumableFunction.Engine.Data.Sqlite -Verbose
Add-Migration -Context "EngineDataContext" -Name "Initial" -Project ResumableFunction.Engine.Data.SqlServer -Verbose

# Force Migration

# Update DataBase
Update-Database -Context "EngineDataContext"

#Remove-Migration 
Remove-Migration -Project ResumableFunction.Engine.Data.SqlServer -Verbose

# Commands Page
https://learn.microsoft.com/en-us/ef/core/cli/powershell