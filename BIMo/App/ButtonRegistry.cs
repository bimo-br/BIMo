using Autodesk.Revit.UI;
using Bimo.Helpers;
using Bimo.Helpers.Attributes;

namespace Bimo.App;

public static class ButtonRegistry
{
    [Category("Info")]
    public static readonly PushButtonData Button1 = new(
        "HelloWorld",
        "Hello World!",
        PathHelper.GetCurrentAssemblyPath(),
        "Bimo.Commands.HelloWorldCommand"
    );

    [Category("Climatização")]
    public static readonly PushButtonData Button2 = new(
        "BtuMonitor",
        "Monitor de BTU",
        PathHelper.GetCurrentAssemblyPath(),
        "Bimo.Commands.BtuMonitorCommand"
    );
}