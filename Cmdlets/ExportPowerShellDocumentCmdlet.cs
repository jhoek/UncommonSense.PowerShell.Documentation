using System.Collections;
using System.Net.WebSockets;

namespace UncommonSense.PowerShell.Documentation;

public abstract class ExportPowerShellDocumentationCmdlet : PSCmdlet
{
    [Parameter()]
    public SwitchParameter OmitIndex { get; set; }



    protected void WriteDocumentation(
        string title,
        string description,
        IEnumerable<string> requirements,
        IEnumerable<string> installationInstructions,
        IEnumerable<CommandInfo> commands,
        Action<string> writeLine
    )
    {
        var powershell = System.Management.Automation.PowerShell.Create();
        var commandsWithHelp =
            commands
                .Where(c => c is CmdletInfo || c is FunctionInfo)
                .ToDictionary(c => c, c => powershell.AddCommand("Get-Help").AddParameter("-Name", c.Name).AddParameter("-Full").Invoke().Single());


        WriteTitle(title, writeLine);
        WriteDescription(description, writeLine);
        WriteRequirements(requirements, writeLine);
        WriteInstallationInstructions(installationInstructions, writeLine);
        WriteIndex(commandsWithHelp, writeLine);
        WriteCommands(commands, writeLine);
        WriteFooter(writeLine);



        // FIXME: versions


    }

    protected void WriteTitle(string title, Action<string> writeLine)
    {
        writeLine.Invoke($"# {title}");
        writeLine.Invoke("");
    }

    protected void WriteDescription(string description, Action<string> writeLine)
    {
        if (!string.IsNullOrEmpty(description))
        {
            writeLine.Invoke($"## Description");
            writeLine.Invoke(description);
            writeLine.Invoke("");
        }
    }

    protected void WriteRequirements(IEnumerable<string> requirements, Action<string> writeLine)
    {
        if ((requirements ?? Array.Empty<string>()).Any())
        {
            writeLine.Invoke($"## Requirements");
            requirements.ToList().ForEach(r => writeLine.Invoke(r));
            writeLine.Invoke("");
        }
    }

    protected void WriteInstallationInstructions(IEnumerable<string> installationInstructions, Action<string> writeLine)
    {
        if (installationInstructions.Any())
        {
            writeLine.Invoke($"## Installation Instructions");
            installationInstructions.ToList().ForEach(i => writeLine.Invoke(i));
            writeLine.Invoke("");
        }
    }

    protected void WriteIndex(Dictionary<CommandInfo, PSObject> commands, Action<string> writeLine)
    {
        var needsIndex = commands.Count() > 1;

        if (needsIndex && !OmitIndex)
        {
            var powershell = System.Management.Automation.PowerShell.Create();

            writeLine.Invoke("## Index");
            writeLine.Invoke("");
            writeLine.Invoke("| Command | Synopsis |");
            writeLine.Invoke("| ------- | -------- |");

            commands
                .Keys
                .Select(k => $"| {k.Name} | {commands[k].Properties["Synopsis"]} |")
                .ToList()
                .ForEach(c => writeLine.Invoke(c));


            /*

s
            foreach ($Command in $CachedCommands)
            {
                $CurrentCommand++

                $HelpInfo = Get-Help $Command -Full
                $LeftColumn = "[$($Command.Name)](#$($Command.Name))"
                $RightColumn = "$(($HelpInfo.Synopsis | Out-String -Width 1200).Trim())" -replace '\n', ' '

                Write-Output "| $LeftColumn | $RightColumn |"
            }

            Write-Output ''
        }

            */

            // FIXME: index
        }
    }

    protected void WriteCommands(IEnumerable<CommandInfo> commands, Action<string> writeLine)
    {
        if (commands.Any())
        {
            writeLine.Invoke($"## Commands");
            writeLine.Invoke("");
        }

        commands.ToList().ForEach(c =>
        {
            // FIXME: mention aliases for cmdlets

            writeLine.Invoke($"<a name='{c.Name}'></a>");
            writeLine.Invoke($"### {c.Name}");
            writeLine.Invoke("");
        });
    }

    protected void WriteFooter(Action<string> writeLine)
    {
        writeLine.Invoke($"Generated {DateTime.Now.ToString("s")}");
    }
}
