using System.Reflection.Metadata;

namespace UncommonSense.PowerShell.Documentation;

[Cmdlet(VerbsData.Export, Nouns.CmdletDocumentation, DefaultParameterSetName = ParameterSet.ToOutputStream)]
[Alias("Get-HelpAsMarkDown")]
public class ExportCmdletDocumentationCmdlet : ExportPowerShellDocumentationCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public CommandInfo[] Command { get; set; }

    [Parameter(Mandatory = true)]
    public string Title { get; set; }

    [Parameter()]
    public string Description { get; set; }

    [Parameter()]
    public string[] Requirement { get; set; }

    [Parameter()]
    public string[] InstallationInstruction { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSet.ToDisk)]
    public string Path { get; set; }

    protected List<CommandInfo> CachedCommands { get; } = new List<CommandInfo>();

    protected override void ProcessRecord() =>
        CachedCommands.AddRange(Command);

    protected override void EndProcessing()
    {
        Action<string> writeLine = null;
        StreamWriter streamWriter = null;

        switch (ParameterSetName)
        {
            case ParameterSet.ToDisk:
                var path = GetUnresolvedProviderPathFromPSPath(Path);
                streamWriter = new StreamWriter(path);
                writeLine = streamWriter.WriteLine;
                break;

            case ParameterSet.ToOutputStream:
                writeLine = WriteObject;
                break;
        }

        WriteDocumentation(
            Title,
            Description,
            Requirement,
            InstallationInstruction,
            CachedCommands,
            writeLine
        );

        if (ParameterSetName == ParameterSet.ToDisk)
            streamWriter.Close();
    }
}