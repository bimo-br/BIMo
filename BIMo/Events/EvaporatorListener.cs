using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimo.Services.MEPServices;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace Bimo.Events
{
    class EvaporatorListener : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;
        static EvaporatorListener _updater = null;
        public List<Double> BtuList => new List<Double>();
        public List<Element> ElementsConnected => new List<Element>();

        public EvaporatorListener(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("865DEADB-9F79-4842-BA70-E0AC459FBEEB"));
        }

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            
            var elements = data.GetModifiedElementIds()
                .Select(id => doc.GetElement(id));

            var btuList = elements
                .Select(e => doc.GetElement(e.GetTypeId()))
                .Select(elementType => elementType.LookupParameter("BTU/H").AsDouble());
 

            var connectedElements = elements
                .Select(element => DuctService.GetAllConnectedElements(element.Id, doc))
                .ToList();

            foreach (var btu in btuList)
            {
                foreach (var connectedElementList in connectedElements)
                    foreach (var connectedElement in connectedElementList)
                        if (connectedElement is Element element)
                        {
                            var param = element.LookupParameter("BTU/H");
                                if (param != null)
                                    param.Set(btu);
                        }
            }
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPSystems; // Prioridade para sistemas MEP
        }

        public string GetUpdaterName()
        {
            return "EvaporatorPhaseMonitor";
        }

        public string GetAdditionalInformation()
        {
            return "Monitora alterações na tipologia ou quantidade de BTU/h das evaporadoras.";
        }

        public static void OnPhaseMonitor(UIApplication uiapp)
        {
            Application app = uiapp.Application;

            if (_updater == null)
            {
                _updater = new EvaporatorListener(app.ActiveAddInId);
            }

            
            if (!UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId()))
            {
                UpdaterRegistry.RegisterUpdater(_updater);
                
                // TODO: melhorar o filtro para monitorar somente as evaporadoras.
                // Define gatilhos para monitorar mudanças em circuitos elétricos
                ElementCategoryFilter mechanicalCategoryFilter = new ElementCategoryFilter(
                    BuiltInCategory.OST_MechanicalEquipment
                );

                UpdaterRegistry.AddTrigger(
                    _updater.GetUpdaterId(),
                    mechanicalCategoryFilter,
                    Element.GetChangeTypeAny()
                );
                
                TaskDialog.Show("Monitor de Eventos", "o monitor foi ativado.");
            }
        }

        public static void ShutOffPhaseMonitor()
        {
            if (_updater != null && UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId()))
            {
                UpdaterRegistry.UnregisterUpdater(_updater.GetUpdaterId());
                _updater = null;
            }
            
            TaskDialog.Show("Monitor de Eventos", "o monitor foi desativado.");
        }
        
        public static EvaporatorListener? GetUpdaterStatus()
        {
            return _updater;
        }
    }
}