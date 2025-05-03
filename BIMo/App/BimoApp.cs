﻿using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Bimo.Helpers;
using Bimo.Helpers.Attributes;

namespace Bimo.App;

public class BimoApp : IExternalApplication
{
    private static readonly string TabName = "BIMo";
    
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
                        pushButton!.LargeImage = ImageHelper.LoadIconLocal(pushButton.Name);
                    }
                }
        }
    }
}