using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace Bimo.Services.MEPServices;

public static class DuctService
{
    public static List<Element> GetAllConnectedElements(ElementId elementId, Document doc)
    {
        var firstElement = doc.GetElement(elementId) as FamilyInstance;

        List<Element> connectedElements = [];
        List<ElementId> processedElementIds = [];
        List<Element> elementsToProcess = [];

        elementsToProcess.Add(firstElement);
        processedElementIds.Add(elementId);
        connectedElements.Add(firstElement);

        while (elementsToProcess.Count > 0)
        {
            Element currentElement = elementsToProcess.First();

            ConnectorSet connectors = null;
            
            // Obtém os conectores do elemento atual
            if (currentElement is MEPCurve mepCurve)
            {
                connectors = mepCurve.ConnectorManager.Connectors;
            }
            else if (currentElement is FamilyInstance familyInstance)
            {
                connectors = familyInstance.MEPModel?.ConnectorManager?.Connectors;
            }

            
            if (connectors != null)
            {
                foreach (Connector connector in connectors)
                {
                    foreach (Connector connectedConnector in connector.AllRefs)
                    {
                        Element connectedElement = connectedConnector.Owner;

                        if (!processedElementIds.Contains(connectedElement.Id))
                        {
                            if (connectedElement is not MechanicalSystem)
                                connectedElements.Add(connectedElement);

                            processedElementIds.Add(connectedElement.Id);
                            elementsToProcess.Add(connectedElement);
                        }
                    }
                }
            }
            elementsToProcess.Remove(currentElement);
        }

        return connectedElements;

    }
}