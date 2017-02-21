<#
.Synopsis
Formats cmdlet help as MarkDown
.Example
Get-Command -Module IDYN.NAV.Automation | Sort-Object -Property Verb | Get-HelpAsMarkDown -Title IDYN.NAV.Automation -Description 'PowerShell cmdlets for IDYN developers.' | clip
Documents module IDYN.NAV.Automation, sorts the functions by verb name, adds a module title and description and copies the resulting text to the clipboard
#>
function Get-HelpAsMarkDown
{
    [CmdletBinding()]
    [OutputType([string[]])]
    Param
    (
        # The command or commands to include in the MarkDown file
        [Parameter(Mandatory,ValueFromPipeLine)]
        $Commands,

        # The title for the MarkDown file
        [string]
        $Title,

        # A description describing this group of commands, e.g. a short module description
        [string]
        $Description,

        # The path of the preface file. This file will be included in the output before the actual command help
        [ValidateScript( { Test-Path $_ } )]
        [string]
        $PrefacePath,

        # The path of the postface file. This file will be included in the output after the actual command help
        [ValidateScript( { Test-Path $_ } )]
        [string]
        $PostfacePath
    )

    Begin
    {
        Add-Type -AssemblyName System.Web

        $CachedCommands = @()
        $Activity = 'Formatting help info as MarkDown'
    }
    Process
    {
        foreach($Command in $Commands)
        {
            $CachedCommands += $Command
        }
    }
    End
    {
        $NoOfCommands = $CachedCommands.Length
        $IndexRequired = $CachedCommands.Length -gt 1

        if ($Title)
        {
            Write-Output "# $Title"
            Write-Output ''
        }
        
        if ($Description)
        {
            Write-Output $Description
            Write-Output ''
        }

        if ($PrefacePath)
        {
            Write-Output (Get-Content -Path $PrefacePath)
            Write-Output ''
        }

        if ($IndexRequired)
        {
            Write-Output '## Index'
            Write-Output ''
            
            Write-Output '| Command | Synopsis |'
            Write-Output '| ------- | -------- |'

            $CurrentCommand = 0
            foreach($Command in $CachedCommands)
            {
                $CurrentCommand++
                Write-Progress -Activity $Activity -CurrentOperation 'Creating index' -PercentComplete ($CurrentCommand / $NoOfCommands * 100)

                $HelpInfo = Get-Help $Command -Full
                $LeftColumn = "[$($Command.Name)](#$($Command.Name))"
                $RightColumn = "$(($HelpInfo.Synopsis | Out-String -Width 1200).Trim())" -replace '\n', ' '

                Write-Output "| $LeftColumn | $RightColumn |"
            }
            
            Write-Output ''
        }

        $CurrentCommand = 0
        foreach($Command in $CachedCommands)
        {
            $CurrentCommand++
            Write-Progress -Activity $Activity -CurrentOperation $Command.Name -PercentComplete ($CurrentCommand / $NoOfCommands * 100)

            $HelpInfo = Get-Help $Command -Full
            
            # Name
            Write-Output "<a name=`"$($Command.Name)`"></a>"
            Write-Output "## $($HelpInfo.Name)"

            # Synopsis
            Write-Output '### Synopsis'
            Write-Output ($HelpInfo.Synopsis | Out-String -Width 1200).Trim()

            # Description
            if ('Description' -in $HelpInfo.PSObject.Properties.Name)
            {
                Write-Output '### Description'
                Write-Output ($HelpInfo.Description | Out-String -Width 1200).Trim()
                Write-Output ''
            }

            # Syntax
            Write-Output '### Syntax'
            Write-Output ``````powershell
            Write-Output ($Command | Get-Command -Syntax).Trim()
            Write-Output ``````

            # Parameters
            Write-Output '### Parameters'
            foreach($Parameter in $HelpInfo.Parameters.Parameter)
            {
                $ParameterText = $Parameter | Out-String -Width 1200
                #Write-Debug $ParameterText
                $ParameterText = $ParameterText -replace '^\s*-', '#### '
                $ParameterText = $ParameterText.Trim()
                $ParameterText = [System.Web.HttpUtility]::HtmlEncode($ParameterText)

                Write-Output $($ParameterText)
            }

            # Examples
            if ($HelpInfo.Examples) 
            # if ('Examples' -in $HelpInfo.PSObject.Properties.Name)
            {
                Write-Output '### Examples'
                foreach($Example in $HelpInfo.Examples.Example)
                {
                    $ExampleTitle = $Example.Title
                    $ExampleTitle = $ExampleTitle -replace '-', ''
                    $ExampleTitle = $ExampleTitle -replace '^\s*', '#### '
                    $ExampleTitle = $ExampleTitle -replace 'EXAMPLE', 'Example'

                    Write-Output $ExampleTitle
                    Write-Output ```````powershell
               
                    if($Example.Code)
                    {
                        Write-Output $Example.Code
                        Write-output ''
                    }

                    Write-Output ```````

                    if ($Example.Remarks)
                    {
                        Write-Output "$(($Example.Remarks | Out-String -Width 1200).Trim())"
                    }
                }
            }
        }

        if ($PostfacePath)
        {
            Write-Output (Get-Content -Path $PostfacePath)
        }

        Write-Output "<div style='font-size:small; color: #ccc'>Generated $(Get-Date -Format 'dd-MM-yyyy HH:mm')</div>"
    }
}