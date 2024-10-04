# (Get-Command Export-CommandDocumentation), (Get-Command Export-ModuleDocumentation) | Export-CommandDocumentation -Title 'Title goes here' -Description 'Description goes here'

Export-ModuleDocumentation -Module (Get-Module UncommonSense.PowerShell.Documentation -ListAvailable) | Set-Content ~/Desktop/output.md

code ~/Desktop/output.md