using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace AscentBoostRevitApp.App;

public class AscentBoostApp : IExternalApplication
{
    private static readonly string TabName = "Ascent";
    private static readonly string PanelName = "Boost";

    public Result OnStartup(UIControlledApplication application)
    {
        application.CreateRibbonTab(TabName);

        // Cria um painel na aba
        var panel = application.CreateRibbonPanel(TabName, PanelName);

        // Caminho para o assembly
        var assemblyPath = Assembly.GetExecutingAssembly().Location;

        // Criar o botão para o HelloWorldCommand
        var buttonData = new PushButtonData(
            "HelloWorldButton",
            "Hello World",
            assemblyPath,
            "AscentBoostRevitApp.Commands.HelloWorldCommand");

        // Verificar se existe pasta de ícones
        var iconsFolder = Path.Combine(Path.GetDirectoryName(assemblyPath), "Resources", "Icons");

        var button = panel.AddItem(buttonData) as PushButton;
        if (button != null)
        {
            // Definir tooltip
            button.ToolTip = "Exibe uma mensagem de Hello World";

            // Adicionar ícone ao botão se existir
            var iconPath = Path.Combine(iconsFolder, "hello_world.png");
            if (File.Exists(iconPath))
            {
                var iconUri = new Uri(iconPath);
                var icon = new BitmapImage(iconUri);
                button.LargeImage = icon;
            }
        }

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}