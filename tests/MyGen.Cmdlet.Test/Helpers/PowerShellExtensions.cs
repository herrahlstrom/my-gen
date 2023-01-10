using System.Management.Automation;
using System.Reflection;

namespace MyGen.Cmdlet.Test.Helpers;

internal static class PowerShellExtensions
{
    public static PowerShell AddCommand<TCommand>(this PowerShell ps) where TCommand : System.Management.Automation.Cmdlet
    {
        var cmdletAttr = typeof(TCommand).GetCustomAttribute<CmdletAttribute>()!;
        string name = $"{cmdletAttr.VerbName}-{cmdletAttr.NounName}";
        ps.AddCommand(new CmdletInfo(name, typeof(TCommand)));
        return ps;
    }
}
