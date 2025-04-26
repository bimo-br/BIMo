using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BIMo.Commands;

[Transaction(TransactionMode.ReadOnly)]
public class HelloWorldCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("HelloWorld", "Hello World");
        return Result.Succeeded;
    }
}