using System.Net.WebSockets;

namespace UncommonSense.PowerShell.Documentation;

public abstract class ExportPowerShellDocumentationCmdlet : PSCmdlet
{
    // FIXME: Maml Documentation attributes
    // FIXME: mention aliases for cmdlets
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
