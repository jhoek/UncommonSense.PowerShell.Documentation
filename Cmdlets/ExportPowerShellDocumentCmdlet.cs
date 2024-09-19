namespace UncommonSense.PowerShell.Documentation;

public abstract class ExportPowerShellDocumentationCmdlet : PSCmdlet
{
    [Parameter()]
    public string Preface { get; set; }

    [Parameter()]
    public string Postface { get; set; }
}

[Cmdlet(VerbsData.Export, Nouns.ModuleDocumentation)]
[Alias("Convert-HelpToMarkDown")]
public class ExportModuleDocumentationCmdlet : ExportPowerShellDocumentationCmdlet
{
    // FIXME: List dependencies, installation instructions

    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public PSModuleInfo[] Module { get; set; }

    [Parameter()]
    [ValidateNotNullOrEmpty()]
    public string Directory { get; set; } = ".";

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
    }
}

[Cmdlet(VerbsData.Export, Nouns.CmdletDocumentation)]
public class ExportCmdletDocumentationCmdlet : ExportPowerShellDocumentationCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public CommandInfo[] Command { get; set; }

    [Parameter(Mandatory = true)]
    public string Title { get; set; }

    [Parameter()]
    public string Description { get; set; }

    [Parameter()]
    [ValidateNotNullOrEmpty()]
    public string Path { get; set; }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
    }
}