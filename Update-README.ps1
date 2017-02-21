. (Join-Path -Path $PSScriptRoot -ChildPath .\Get-HelpAsMarkDown.ps1)

Get-Command Get-HelpAsMarkDown | Get-HelpAsMarkDown | Out-File .\README.md -Encoding utf8