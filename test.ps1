# (Get-Command Export-CommandDocumentation), (Get-Command Export-ModuleDocumentation) | Export-CommandDocumentation -Title 'Title goes here' -Description 'Description goes here'

$Module = Get-Module UncommonSense.Nrc -ListAvailable | Import-Module
Export-ModuleDocumentation -Module $Module | Set-Content ~/Desktop/output.md

code ~/Desktop/output.md