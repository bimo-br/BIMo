using System.Reflection;
using AscentBoostRevitApp.Helpers;
using AscentBoostRevitApp.Helpers.Attributes;
using Autodesk.Revit.UI;

namespace AscentBoostRevitApp.App;

public class AscentBoostApp : IExternalApplication
{
    private static readonly string TabName = "Ascent";
    private static readonly string PanelName = "Boost";

    public Result OnStartup(UIControlledApplication application)
    {
        application.CreateRibbonTab(TabName);

        CreatePanelWithButtons(application, TabName);

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }

    private void CreatePanelWithButtons(
        UIControlledApplication application,
        string tabName)
    {
        var buttonRegistry = typeof(ButtonRegistry);


        var buttonCategories = CategoryHelper.GetCategoryValues(buttonRegistry);
        var buttonsFieldInfo = buttonRegistry.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var category in buttonCategories)
        {
            var ribbonPanel = application.CreateRibbonPanel(tabName, category);
            foreach (var buttonFieldInfo in buttonsFieldInfo)
                if (buttonFieldInfo.GetValue(null) is PushButtonData buttonData)
                {
                    var categoryAttribute = buttonFieldInfo.GetCustomAttribute<CategoryAttribute>();
                    if (categoryAttribute != null && categoryAttribute.Category == category)
                    {
                        var pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
                        pushButton.LargeImage = ImageHelper.LoadIconLocal(pushButton.Name);
                    }
                }
        }
    }
}