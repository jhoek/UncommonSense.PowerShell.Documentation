using System.Net.WebSockets;

namespace UncommonSense.PowerShell.Documentation;

public abstract class ExportPowerShellDocumentationCmdlet : PSCmdlet
{
    [Parameter()]
    public string Preface { get; set; }

    [Parameter()]
    public string Postface { get; set; }

    [Parameter()]
    public SwitchParameter OmitIndex { get; set; }

    protected void WriteDocumentation(
        string title,
        string description,
        IEnumerable<CommandInfo> commands,
        Action<string> writeLine
    )
    {
        commands = commands.Where(c => c is CmdletInfo || c is FunctionInfo).OrderBy(c => c.Name);

        writeLine.Invoke($"# {title}");
        writeLine.Invoke("");
        writeLine.Invoke($"## Description");
        writeLine.Invoke("");
        writeLine.Invoke(description);
        writeLine.Invoke("");

        commands.ToList().ForEach(c =>
        {
            writeLine.Invoke($"<a name='{c.Name}'></a>");
            writeLine.Invoke($"## {c.Name}");
            writeLine.Invoke("");
        });


        // FIXME: List dependencies, installation instructions
        // FIXME: Index if necessary and not omitted
        // FIXME: mention aliases for cmdlets

        writeLine.Invoke($"Generated {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");
    }
}
