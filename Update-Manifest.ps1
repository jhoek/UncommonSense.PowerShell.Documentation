$ManifestPath = Join-Path -Path $PSScriptRoot -ChildPath UncommonSense.Powershell.Documentation.psd1

Import-Module $ManifestPath -Force

Update-ModuleManifest `
    -Path $ManifestPath `
    -FunctionsToExport (Get-Command -Module UncommonSense.PowerShell.Documentation -CommandType Function).Name