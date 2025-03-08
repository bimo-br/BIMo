using System.Reflection;

namespace AscentBoostRevitApp.Helpers;

/// <summary>
///     Esse helper sever para manipular caminhos e diretórios úteis da aplicação.
/// </summary>
public static class PathHelper
{
    /// <summary>
    ///     Obtém o diretório base da aplicação
    /// </summary>
    /// <returns>Caminho do diretório base da aplicação</returns>
    public static string? GetAppDirectory()
    {
        return Path.GetDirectoryName(GetCurrentAssemblyPath());
    }

    /// <summary>
    ///     Obtém o caminho completo da DLL atual
    /// </summary>
    /// <returns>Caminho completo da DLL atual</returns>
    public static string? GetCurrentAssemblyPath()
    {
        return Assembly.GetExecutingAssembly().Location;
    }

    /// <summary>
    ///     Obtém o caminho completo para um arquivo de ícone no caminho padrão da aplicação
    /// </summary>
    /// <param name="iconName">Nome do arquivo do ícone</param>
    /// <returns>Caminho completo para o arquivo de ícone</returns>
    public static string GetIconPath(string iconName, string format = "png")
    {
        iconName = $"{iconName}.{format}";
        return Path.Combine(GetIconsDirectory(), iconName);
    }

    /// <summary>
    ///     Obtém o caminho para o diretório de ícones
    /// </summary>
    /// <returns>Caminho completo para o diretório de ícones</returns>
    public static string GetIconsDirectory()
    {
        return Path.Combine(GetResourcesDirectory(), "Icons");
    }

    /// <summary>
    ///     Obtém o caminho para o diretório de recursos
    /// </summary>
    /// <returns>Caminho completo para o diretório de recursos</returns>
    public static string GetResourcesDirectory()
    {
        return Path.Combine(GetAppDirectory(), "Resources");
    }
}