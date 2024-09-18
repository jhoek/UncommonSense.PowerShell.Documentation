using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;

namespace UncommonSense.PowerShell.Documentation;

[Cmdlet(VerbsData.Export, Nouns.ModuleDocumentation, DefaultParameterSetName = ParameterSets.FromModuleToText)]
[Alias("Convert-HelpToMarkDown")]
public class ExportModuleDocumentationCmdlet : PSCmdlet
{
    public static class ParameterSets
    {
        public const string FromModuleToFile = nameof(FromModuleToFile);
        public const string FromModuleToText = nameof(FromModuleToText);
        public const string FromMethodsToFile = nameof(FromMethodsToFile);
        public const string FromMethodsToText = nameof(FromMethodsToText);
    }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromModuleToFile, ValueFromPipeline = true)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromModuleToText, ValueFromPipeline = true)]
    public PSModuleInfo[] Module { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToFile, ValueFromPipeline = true)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToText, ValueFromPipeline = true)]
    public CommandInfo[] Command { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToFile)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToText)]
    public string Title { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToFile)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToText)]
    public string Description { get; set; }

    [Parameter()]
    public string Preface { get; set; }

    [Parameter()]
    public string Postface { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromModuleToFile)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToFile)]
    public string Path { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromModuleToFile)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.FromMethodsToFile)]
    public string PassThru { get; set; }

    protected List<PSModuleInfo> ModuleInfoCache = new List<PSModuleInfo>();
    protected List<CommandInfo> CommandInfoCache = new List<CommandInfo>();

    protected override void ProcessRecord()
    {
        ModuleInfoCache.AddRange(Module ?? Array.Empty<PSModuleInfo>());
        CommandInfoCache.AddRange(Command ?? Array.Empty<CommandInfo>());
    }

    protected string EffectiveTitle(PSModuleInfo moduleInfo) => Title ?? moduleInfo.Name;
    protected

    protected override void EndProcessing()
    {

    }





    // FIXME: List dependencies, installation instructions
}
