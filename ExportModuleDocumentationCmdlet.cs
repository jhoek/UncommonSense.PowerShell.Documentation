using System.Management.Automation;

namespace UncommonSense.PowerShell.Documentation;

[Cmdlet(VerbsData.Export, Nouns.ModuleDocumentation)]
[Alias("Convert-HelpToMarkDown")]
public class ExportModuleDocumentationCmdlet : PSCmdlet
{


    // FIXME: Preface/postface from file or literal
    // FIXME: List dependencies, installation instructions
    // FIXME: To file or as text
    // FIXME: From moduleinfo or from methodinfo[]
}
