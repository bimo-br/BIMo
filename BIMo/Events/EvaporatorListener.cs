using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace BIMo.Events
{
    /// <summary>
    /// Classe responsável por monitorar mudanças nas fases de circuitos elétricos em modelos Revit.
    /// Implementa a interface <see cref="IUpdater"/> para registrar e executar atualizações automaticamente.
    /// </summary>
    class EvaporatorListener : IUpdater
    {
        // Identificação do Add-In e do Updater
        static AddInId _appId;
        static UpdaterId _updaterId;

        // Referência global para o updater
        static EvaporatorListener _updater = null;

        /// <summary>
        /// Inicializa uma nova instância do PhaseListener, associando um identificador único ao Add-In.
        /// </summary>
        /// <param name="id">Identificação do Add-In Revit.</param>
        public EvaporatorListener(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("865DEADB-9F79-4842-BA70-E0AC459FBEEB"));
        }

        /// <summary>
        /// Método chamado automaticamente pelo Revit sempre que elementos monitorados são alterados.
        /// Analisa as mudanças feitas nos circuitos elétricos e atualiza o valor da fase analisada.
        /// </summary>
        /// <param name="data">
        /// Dados enviados pelo Revit contendo informações sobre os elementos alterados no modelo.
        /// </param>
        public void Execute(UpdaterData data)
        {
            TaskDialog.Show("Monitor de EveNtos", "Executou o updater, equipamento mecânico alterao.");
        }

        /// <summary>
        /// Retorna o identificador único do Updater associado a esta instância.
        /// </summary>
        /// <returns>Um identificador do tipo <see cref="UpdaterId"/>.</returns>
        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        /// <summary>
        /// Determina a prioridade das mudanças que este updater deve monitorar.
        /// Neste caso, prioriza sistemas MEP (e.g., circuitos, conectores).
        /// </summary>
        /// <returns>Um valor do tipo <see cref="ChangePriority"/> configurado como MEPSystems.</returns>
        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPSystems; // Prioridade para sistemas MEP
        }

        /// <summary>
        /// Retorna o nome descritivo do Updater.
        /// </summary>
        /// <returns>O nome do Updater, como "PhaseChangeAlarm".</returns>
        public string GetUpdaterName()
        {
            return "EvaporatorPhaseMonitor";
        }

        /// <summary>
        /// Retorna informações adicionais ou descritivas sobre o Updater.
        /// </summary>
        /// <returns>Uma string explicando a funcionalidade do updater.</returns>
        public string GetAdditionalInformation()
        {
            return "Monitora alterações na tipologia ou quantidade de BTU/h das evaporadoras.";
        }

        /// <summary>
        /// Ativa o monitoramento para mudanças no estado do parâmetro de fase dos circuitos elétricos.
        /// </summary>
        /// <param name="uiapp">A aplicação controlada da interface do usuário.</param>
        public static void OnPhaseMonitor(UIApplication uiapp)
        {
            Application app = uiapp.Application;

            // Inicializa o updater se ainda não estiver configurado
            if (_updater == null)
            {
                _updater = new EvaporatorListener(app.ActiveAddInId);
            }

            // Verifica se o updater já está registrado no Revit
            if (!UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId()))
            {
                // Registra o updater se ainda não estiver ativo
                UpdaterRegistry.RegisterUpdater(_updater);

                // Define gatilhos para monitorar mudanças em circuitos elétricos
                ElementCategoryFilter mechanicalCategoryFilter = new ElementCategoryFilter(
                    BuiltInCategory.OST_MechanicalEquipment
                );

                UpdaterRegistry.AddTrigger(
                    _updater.GetUpdaterId(),
                    mechanicalCategoryFilter,
                    Element.GetChangeTypeAny() // Monitora qualquer tipo de mudança
                );
                
                TaskDialog.Show("Monitor de Evetos", "o monitor foi ativado.");
            }
        }

        /// <summary>
        /// Desativa o monitoramento das alterações de fase ao limpar o registro do Updater.
        /// </summary>
        public static void ShutOffPhaseMonitor()
        {
            if (_updater != null && UpdaterRegistry.IsUpdaterRegistered(_updater.GetUpdaterId()))
            {
                // Desregistra o Updater para parar o monitoramento
                UpdaterRegistry.UnregisterUpdater(_updater.GetUpdaterId());
                _updater = null; // Limpa a referência ao updater
            }
            
            TaskDialog.Show("Monitor de Evetos", "o monitor foi desativado.");
        }
        
        public static EvaporatorListener? GetUpdaerStatus()
        {
            return _updater;
        }
    }
}