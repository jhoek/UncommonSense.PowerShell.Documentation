namespace UncommonSense.PowerShell.Documentation;

[Cmdlet(VerbsData.Export, Nouns.CmdletDocumentation, DefaultParameterSetName = ParameterSet.ToOutputStream)]
public class ExportCmdletDocumentationCmdlet : ExportPowerShellDocumentationCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public CommandInfo[] Command { get; set; }

    [Parameter(Mandatory = true)]
    public string Title { get; set; }

    [Parameter()]
    public string Description { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSet.ToDisk)]
    [ValidateNotNullOrEmpty()]
    public string Path { get; set; }

    protected List<CommandInfo> CachedCommands { get; } = new List<CommandInfo>();
    protected StreamWriter StreamWriter { get; set; }

    protected override void BeginProcessing()
    {
        switch (ParameterSetName)
        {
            case ParameterSet.ToDisk:
                Path = GetUnresolvedProviderPathFromPSPath(Path);
                StreamWriter = new StreamWriter(Path);
                WriteLine = StreamWriter.WriteLine;
                break;

            case ParameterSet.ToOutputStream:
                WriteLine = WriteObject;
                break;
        }

        WriteModuleInfo(Title, Description);

        // FIXME: Index if necessary and not omitted
    }

    protected override void ProcessRecord()
    {
        Command.ToList().ForEach(c => WriteCommandInfo(c));
    }

    protected override void EndProcessing()
    {
        WriteFooter();

        if (ParameterSetName == ParameterSet.ToDisk)
            StreamWriter.Close();
    }
}