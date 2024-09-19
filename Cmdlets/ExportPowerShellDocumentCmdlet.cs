using System.Net.WebSockets;

namespace UncommonSense.PowerShell.Documentation;

public abstract class ExportPowerShellDocumentationCmdlet : PSCmdlet
{
    // FIXME: Maml Documentation attributes
    // FIXME:
    // [Parameter()]
    // public string Preface { get; set; }

    // [Parameter()]
    // public string Postface { get; set; }

    [Parameter()]
    public SwitchParameter OmitIndex { get; set; }

    protected Action<string> WriteLine { get; set; }

    protected void WriteModuleInfo(string title, string description)
    {
        WriteLine.Invoke($"# {title}");
        WriteLine.Invoke("");
        WriteLine.Invoke($"## Description");
        WriteLine.Invoke("");
        WriteLine.Invoke(description);
        WriteLine.Invoke("");
    }

    protected void WriteCommandInfo(CommandInfo commandInfo)
    {
        WriteLine.Invoke($"<a name='{commandInfo.Name}'></a>");
        WriteLine.Invoke($"## {commandInfo.Name}");
        WriteLine.Invoke("");
    }

    protected void WriteFooter()
    {
        WriteLine.Invoke($"Generated {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");
    }
}

public static class ParameterSet
{
    public const string ToOutputStream = nameof(ToOutputStream);
    public const string ToDisk = nameof(ToDisk);
}

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
        module.ExportedCmdlets.Values.ToList().ForEach(WriteCommandInfo);
        WriteFooter();

        if (ParameterSetName == ParameterSet.ToDisk)
            streamWriter.Close();
    }
}

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

    protected StreamWriter streamWriter;

    protected override void BeginProcessing()
    {
        switch (ParameterSetName)
        {
            case ParameterSet.ToDisk:
                Path = GetUnresolvedProviderPathFromPSPath(Path);
                streamWriter = new StreamWriter(Path);
                WriteLine = streamWriter.WriteLine;
                break;

            case ParameterSet.ToOutputStream:
                WriteLine = WriteObject;
                break;
        }

        WriteModuleInfo(Title, Description);
    }

    protected override void ProcessRecord()
    {
        Command.ToList().ForEach(c => WriteCommandInfo(c));
    }

    protected override void EndProcessing()
    {
        WriteFooter();

        if (ParameterSetName == ParameterSet.ToDisk)
            streamWriter.Close();
    }
}