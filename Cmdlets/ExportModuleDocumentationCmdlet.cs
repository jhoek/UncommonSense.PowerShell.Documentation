namespace UncommonSense.PowerShell.Documentation;

[Cmdlet(VerbsData.Export, Nouns.ModuleDocumentation, DefaultParameterSetName = ParameterSet.ToOutputStream)]
[Alias("Convert-HelpToMarkDown")]
public class ExportModuleDocumentationCmdlet : ExportPowerShellDocumentationCmdlet
{
    // FIXME: List dependencies, installation instructions

    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public PSModuleInfo[] Module { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSet.ToDisk)]
    [ValidateNotNullOrEmpty()]
    public string Directory { get; set; } = ".";

    protected override void BeginProcessing()
    {
        if (ParameterSetName == ParameterSet.ToDisk)
            Directory = GetUnresolvedProviderPathFromPSPath(Directory);
    }

    protected override void ProcessRecord() => Module.ToList().ForEach(m => ProcessModule(m));

    protected void ProcessModule(PSModuleInfo module)
    {
        StreamWriter streamWriter = null;

        switch (ParameterSetName)
        {
            case ParameterSet.ToDisk:
                var fileName = Path.Combine(Directory, module.Name);
                streamWriter = new StreamWriter(fileName);
                WriteLine = streamWriter.WriteLine;
                break;

            case ParameterSet.ToOutputStream:
                WriteLine = WriteObject;
                break;
        }

        WriteModuleInfo(module.Name, module.Description);
        // FIXME: Index if necessary and not omitted
        module.ExportedCmdlets.Values.ToList().ForEach(WriteCommandInfo);
        WriteFooter();

        if (ParameterSetName == ParameterSet.ToDisk)
            streamWriter.Close();
    }
}
