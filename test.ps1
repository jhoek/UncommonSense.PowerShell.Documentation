(Get-Command Export-CommandDocumentation), (Get-Command Export-ModuleDocumentation)
| Export-CommandDocumentation -Title 'Title goes here' -Description 'Description goes here'
| Set-Content -Path ~/Desktop/command.md

code ~/Desktop/command.md

$Module = Get-Module UncommonSense.Nrc -ListAvailable | Import-Module -PassThru
Export-ModuleDocumentation -Module $Module
| Set-Content -Path ~/Desktop/module.md

code ~/Desktop/module.md