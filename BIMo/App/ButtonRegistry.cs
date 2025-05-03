using Autodesk.Revit.UI;
using BIMo.Helpers;
using BIMo.Helpers.Attributes;

namespace BIMo.App;

public static class ButtonRegistry
{
    [Category("Info")]
    public static readonly PushButtonData Button1 = new(
        "HelloWorld",
        "Hello World!",
        PathHelper.GetCurrentAssemblyPath(),
        "BIMo.Commands.HelloWorldCommand"
    );

    [Category("Climatização")]
    public static readonly PushButtonData Button2 = new(
        "BtuMonitor",
        "Monitor de BTU",
        PathHelper.GetCurrentAssemblyPath(),
        "BIMo.Commands.BtuMonitorCommand"
    );
}