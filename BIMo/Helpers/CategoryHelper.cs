using System.Reflection;
using BIMo.Helpers.Attributes;

namespace BIMo.Helpers;

public static class CategoryHelper
{
    /// <summary>
    ///     Obtém todas as categorias únicas usadas nos campos estáticos de uma classe específica
    /// </summary>
    /// <param name="type">O tipo da classe que contém os campos categorizados</param>
    /// <returns>Uma lista de categorias únicas</returns>
    public static List<string> GetCategoryValues(Type type)
    {
        var categories = new List<string>();

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var field in fields)
        {
            var categoryAttribute = field.GetCustomAttribute<CategoryAttribute>();
            if (categoryAttribute != null)
                if (!categories.Contains(categoryAttribute.Category))
                    categories.Add(categoryAttribute.Category);
        }

        return categories;
    }
}