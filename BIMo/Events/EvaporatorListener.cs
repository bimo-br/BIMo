using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimo.Services.MEPServices;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace Bimo.Events
{
    class EvaporatorListener : IUpdater
    {
        static AddInId? _appId;
        static UpdaterId? _updaterId;
        static EvaporatorListener? _updater = null;

        public EvaporatorListener(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("865DEADB-9F79-4842-BA70-E0AC459FBEEB"));
        }

        public void Execute(UpdaterData data)
        {
            Autodesk.Revit.DB.Document doc = data.GetDocument();

            var evaporators = data.GetModifiedElementIds()
                .Select(id => doc.GetElement(id))
                .Where(e => 
                    e is FamilyInstance instance && e.Category.BuiltInCategory 
                    == BuiltInCategory.OST_MechanicalEquipment && IsEvaporator(instance))
                .ToList();

            foreach (var evaporator in evaporators)
            {

                Parameter evaporatorParameter = GetBtuParameter(evaporator, doc);
                var connectedElements = DuctService.GetAllConnectedElements(evaporator.Id, doc);

                foreach (var connectedElement in connectedElements)
                {
                    if (connectedElement == null || connectedElement.Id == evaporator.Id) continue;
                    
                    Parameter btuParam = connectedElement.LookupParameter("BTU/H");
                    btuParam.Set(evaporatorParameter.AsDouble());
                }
            }
        }
        private static bool IsEvaporator(FamilyInstance element)
        {
            if (element.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString().ToUpper().Contains("EVAPORADORA"))
            {
                return true;
            }
            return false;
        }

        private static Parameter GetBtuParameter(Element element, Document doc)
        {

            var elementType = doc.GetElement(element.GetTypeId());
            return elementType.LookupParameter("BTU/H");
        }


        public static void OnStartMonitor(UIApplication uiapp)
        {
            Application app = uiapp.Application;

            if (_updater == null) _updater = new EvaporatorListener(app.ActiveAddInId);

            Document doc = uiapp.ActiveUIDocument.Document;
            if (!UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId()))
            {
                UpdaterRegistry.RegisterUpdater(_updater, doc);

                ElementCategoryFilter mechanicalCategoryFilter = new ElementCategoryFilter(
                    BuiltInCategory.OST_MechanicalEquipment
                );

                UpdaterRegistry.AddTrigger(
                    _updater.GetUpdaterId(),
                    doc,
                    mechanicalCategoryFilter,
                    Element.GetChangeTypeAny()
                );

                TaskDialog.Show("Monitor de Eventos", "O monitor foi ativado.");
            }
        }

        public static void ShutOffMonitor()
        {
            if (_updater != null && UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId()))
            {
                UpdaterRegistry.UnregisterUpdater(_updaterId);
                _updater = null;
            }

            TaskDialog.Show("Monitor de Eventos", "O monitor foi desativado.");
        }

        public UpdaterId GetUpdaterId() => _updaterId!;

        public ChangePriority GetChangePriority() => ChangePriority.MEPSystems;

        public string GetUpdaterName() => "EvaporatorMonitor";

        public string GetAdditionalInformation() => "Monitora alterações na tipologia ou quantidade de BTU/h das evaporadoras.";

        public static EvaporatorListener? GetUpdaterStatus() => _updater;
    }
}