namespace Bimo.Helpers.Attributes;

/// <summary>
///     Atributo personalizado usado para categorizar elementos em classes estáticas.
///     Este atributo é aplicado a campos (fields) para associá-los a apenas UMA categoria específica.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class CategoryAttribute : Attribute
{
    public CategoryAttribute(string category)
    {
        Category = category;
    }

    public string Category { get; }
}