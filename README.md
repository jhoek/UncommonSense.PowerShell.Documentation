<a name="Get-HelpAsMarkDown"></a>
## Get-HelpAsMarkDown
### Synopsis
Formats PowerShell cmdlet help as MarkDown
### Syntax
```powershell
Get-HelpAsMarkDown [-Commands] <Object> [[-Title] <string>] [[-Description] <string>] [[-PrefacePath] <string>] [[-PostfacePath] <string>] [<CommonParameters>]
```
### Parameters
#### Commands &lt;Object&gt;
    The command or commands to include in the MarkDown file
    
    Required?                    true
    Position?                    1
    Default value                
    Accept pipeline input?       true (ByValue)
    Accept wildcard characters?  false
#### Title &lt;String&gt;
    The title for the MarkDown file
    
    Required?                    false
    Position?                    2
    Default value                
    Accept pipeline input?       false
    Accept wildcard characters?  false
#### Description &lt;String&gt;
    A description describing this group of commands, e.g. a short module description
    
    Required?                    false
    Position?                    3
    Default value                
    Accept pipeline input?       false
    Accept wildcard characters?  false
#### PrefacePath &lt;String&gt;
    The path of the preface file. This file will be included in the output before the actual command help
    
    Required?                    false
    Position?                    4
    Default value                
    Accept pipeline input?       false
    Accept wildcard characters?  false
#### PostfacePath &lt;String&gt;
    The path of the postface file. This file will be included in the output after the actual command help
    
    Required?                    false
    Position?                    5
    Default value                
    Accept pipeline input?       false
    Accept wildcard characters?  false
### Examples
#### Example 1 
```powershell
Get-Command -Module IDYN.NAV.Automation | Sort-Object -Property Verb | Get-HelpAsMarkDown -Title IDYN.NAV.Automation -Description 'PowerShell cmdlets for IDYN developers.' | clip

```
Documents module IDYN.NAV.Automation, sorts the functions by verb name, adds a module title and description and copies the resulting text to the clipboard
<div style='font-size:small; color: #ccc'>Generated 21-02-2017 19:54</div>
