<#
.Synopsis
Formats module help as MarkDown
#>
function Export-ModuleDocumentation
{
    [OutputType([string[]])]
    param
    (
        [Parameter(Mandatory)]
        [psmoduleinfo]$Module,

        # The path of the preface file. This file will be included in the output before the actual command help
        [ValidateScript( { Test-Path $_ } )]
        [string]
        $PrefacePath,

        # The path of the postface file. This file will be included in the output after the actual command help
        [ValidateScript( { Test-Path $_ } )]
        [string]
        $PostfacePath
    )

    $Parameters = @{
        Commands = $Module.ExportedCommands.Values | Where-Object { $_ -is [System.Management.Automation.FunctionInfo] -or $_ -is [System.Management.Automation.CmdletInfo] }
    }

    $Requirements = ($Module.RequiredModules).Name + $Module.RequiredAssemblies

    if ($Module.Name) { $Parameters.Title = $Module.Name }
    if ($Module.Description) { $Parameters.Description = $Module.Description }
    if ($Module.LicenseUri) { $Parameters.LicenseUri = $Module.LicenseUri}
    if ($Module.Copyright) { $Parameters.Copyright = $Module.Copyright }
    if ($Requirements) { $Parameters.Requirement = $Requirements }
    if ($PrefacePath) { $Parameters.PrefacePath = $PrefacePath }
    if ($PostfacePath) { $Parameters.PostfacePath = $PostfacePath }

    Export-CommandDocumentation @Parameters
}