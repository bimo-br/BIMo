using System.Windows.Media.Imaging;

namespace AscentBoostRevitApp.Helpers;

/// <summary>
///     Utilitário para carregamento de imagens na aplicação
/// </summary>
public static class ImageHelper
{
    /// <summary>
    ///     Carrega um ícone da pasta Resources/Icons
    /// </summary>
    /// <param name="assemblyPath">Caminho do assembly</param>
    /// <param name="iconName">Nome do arquivo de ícone</param>
    /// <returns>Ícone carregado ou null se não encontrado</returns>
    public static BitmapImage LoadIconFromAssembly(string assemblyPath, string iconName)
    {
        var iconsFolder = Path.Combine(Path.GetDirectoryName(assemblyPath), "Resources", "Icons");
        var iconPath = Path.Combine(iconsFolder, iconName);
        return LoadImage(iconPath);
    }

    /// <summary>
    ///     Carrega uma imagem do caminho especificado
    /// </summary>
    /// <param name="imagePath">Caminho do arquivo de imagem</param>
    /// <returns>Imagem carregada ou null se falhar</returns>
    public static BitmapImage LoadImage(string imagePath)
    {
        if (File.Exists(imagePath))
            try
            {
                var iconUri = new Uri(imagePath);
                var icon = new BitmapImage(iconUri);
                return icon;
            }
            catch (Exception)
            {
                return null;
            }

        return null;
    }
}