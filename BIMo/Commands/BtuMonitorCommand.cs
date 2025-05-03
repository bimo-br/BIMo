using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMo.Events;

namespace BIMo.Commands;

[Transaction(TransactionMode.ReadOnly)]
public class BtuMonitorCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        if (EvaporatorListener.GetUpdaerStatus() == null)
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