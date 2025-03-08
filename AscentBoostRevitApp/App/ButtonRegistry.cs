using AscentBoostRevitApp.Helpers;
using AscentBoostRevitApp.Helpers.Attributes;
using Autodesk.Revit.UI;

namespace AscentBoostRevitApp.App;

public static class ButtonRegistry
{
    [Category("Boost")] public static readonly PushButtonData Button1 = new(
        "HelloWorld",
        "Hello World!",
        PathHelper.GetCurrentAssemblyPath(),
        "AscentBoostRevitApp.Commands.HelloWorldCommand"
    );

    [Category("Boost")] public static readonly PushButtonData Button2 = new(
        "HelloWorld...",
        "Hello World...",
        PathHelper.GetCurrentAssemblyPath(),
        "AscentBoostRevitApp.Commands.HelloWorldCommand"
    );
}