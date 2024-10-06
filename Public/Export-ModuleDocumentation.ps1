<#
.Synopsis
Formats module help as MarkDown
#>
function Export-ModuleDocumentation
{
    [OutputType([string[]])]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline)]
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

    process
    {
        $CurrentModule = $_

        $Parameters = @{
            Commands = $CurrentModule.ExportedCommands.Values | Where-Object { $_ -is [System.Management.Automation.FunctionInfo] -or $_ -is [System.Management.Automation.CmdletInfo] }
        }

        $Requirements = ($CurrentModule.RequiredModules).Name + $CurrentModule.RequiredAssemblies

        if ($CurrentModule.Name) { $Parameters.Title = $CurrentModule.Name }
        if ($CurrentModule.Description) { $Parameters.Description = $CurrentModule.Description }
        if ($CurrentModule.LicenseUri) { $Parameters.LicenseUri = $CurrentModule.LicenseUri}
        if ($CurrentModule.Copyright) { $Parameters.Copyright = $CurrentModule.Copyright }
        if ($Requirements) { $Parameters.Requirement = $Requirements }
        if ($PrefacePath) { $Parameters.PrefacePath = $PrefacePath }
        if ($PostfacePath) { $Parameters.PostfacePath = $PostfacePath }

        Export-CommandDocumentation @Parameters
    }
}