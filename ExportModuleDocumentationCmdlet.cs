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






    // FIXME: Title and Description parameters when passing in methods
    // FIXME: Preface/postface from file or literal
    // FIXME: List dependencies, installation instructions
    // FIXME: To file or as text
    // FIXME: From moduleinfo or from methodinfo[]; output resp. to directory or path
}
