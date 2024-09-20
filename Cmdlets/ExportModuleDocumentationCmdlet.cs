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
                        var fileName = Path.Combine(directory, m.Name);
                        streamWriter = new StreamWriter(fileName);
                        writeLine = streamWriter.WriteLine;
                        break;

                    case ParameterSet.ToOutputStream:
                        writeLine = WriteObject;
                        break;
                }

                WriteDocumentation(
                    m.Name,
                    m.Description,
                    m.ExportedCommands.Values,
                    writeLine
                );

                if (ParameterSetName == ParameterSet.ToDisk)
                    streamWriter.Close();
            });
}
