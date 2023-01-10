if (Get-Module -Name MyGen.Cmdlet) {
    Write-Host "Remove-Module MyGen.Cmdlets"
    Remove-Module MyGen.Cmdlets
} 

dotnet publish -c Release .\src\MyGen.Cmdlets\MyGen.Cmdlets.csproj

Write-Host "Import-Module MyGen.Cmdlets"
$fullpath = Resolve-Path src\MyGen.Cmdlets\bin\Release\net7.0\publish\MyGen.Cmdlets.dll
Import-Module $fullpath.ToString()