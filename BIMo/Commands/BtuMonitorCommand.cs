using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimo.Events;

namespace Bimo.Commands;

[Transaction( TransactionMode.ReadOnly )]
[Regeneration( RegenerationOption.Manual )]
public class BtuMonitorCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Document doc = commandData.Application.ActiveUIDocument.Document;
        
        if (EvaporatorListener.GetUpdaterStatus() == null)
        {
            EvaporatorListener.OnPhaseMonitor(commandData.Application);
        }
        else
        {
            EvaporatorListener.ShutOffPhaseMonitor();
        }
        
        return Result.Succeeded;
    }
}