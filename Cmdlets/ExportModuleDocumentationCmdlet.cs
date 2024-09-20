namespace UncommonSense.PowerShell.Documentation;

[Cmdlet(VerbsData.Export, Nouns.ModuleDocumentation, DefaultParameterSetName = ParameterSet.ToOutputStream)]
[Alias("Convert-HelpToMarkDown")]
public class ExportModuleDocumentationCmdlet : ExportPowerShellDocumentationCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public PSModuleInfo[] Module { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSet.ToDisk)]
    [ValidateNotNullOrEmpty()]
    public string Directory { get; set; } = ".";

    [Parameter()]
    public SwitchParameter OmitInstallationInstructions { get; set; }

    protected override void ProcessRecord() =>
        Module
            .ToList()
            .ForEach(m =>
            {
                Action<string> writeLine = null;
                StreamWriter streamWriter = null;

                switch (ParameterSetName)
                {
                    case ParameterSet.ToDisk:
                        var directory = GetUnresolvedProviderPathFromPSPath(Directory);
                        var fileName = $"{m.Name}.md";
                        var filePath = Path.Combine(directory, fileName);
                        streamWriter = new StreamWriter(filePath);
                        writeLine = streamWriter.WriteLine;
                        break;

                    case ParameterSet.ToOutputStream:
                        writeLine = WriteObject;
                        break;
                }

                WriteDocumentation(
                    m.Name,
                    m.Description,
                    m.RequiredModules.Select(m => m.Name),
                    OmitInstallationInstructions ? [] : ["```powershell", $"Install-Module '{m.Name}'", "```"],
                    m.ExportedCommands.Values,
                    writeLine
                );

                if (ParameterSetName == ParameterSet.ToDisk)
                    streamWriter.Close();
            });
}
