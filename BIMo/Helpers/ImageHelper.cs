using System.Windows.Media.Imaging;

namespace Bimo.Helpers;

/// <summary>
///     Utilitário para carregamento de imagens na aplicação
/// </summary>
public static class ImageHelper
{
    /// <summary>
    ///     Carrega um ícone da pasta Resources/Icons
    /// </summary>
    /// <param name="iconName">Nome do arquivo de ícone</param>
    /// <returns>Ícone carregado ou null se não encontrado</returns>
    public static BitmapSource? LoadIconLocal(string iconName)
    {
        var iconPath = PathHelper.GetIconPath(iconName);
        return LoadImageByPath(iconPath);
    }

    /// <summary>
    ///     Carrega uma imagem do caminho especificado
    /// </summary>
    /// <param name="imagePath">Caminho do arquivo de imagem</param>
    /// <returns>Imagem carregada ou null se falhar</returns>
    public static BitmapSource? LoadImageByPath(string iconPath)
    {
        const int ICON_LARGE = 32;
        const int DEFAULT_DPI = 96;

        try
        {
            if (!File.Exists(iconPath))
            {
                Console.WriteLine($"Ícone não foi encontrado: {iconPath}");
                return null;
            }

            // Carregar a imagem como BitmapSource
            BitmapSource bitmapSource;
            using (var stream = new FileStream(iconPath, FileMode.Open, FileAccess.Read))
            {
                var decoder = new PngBitmapDecoder(
                    stream,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad);

                bitmapSource = decoder.Frames[0];
            }

            // Propriedades da imagem
            var currentImageSize = bitmapSource.PixelWidth;
            var imageFormat = bitmapSource.Format;
            var bytesPerPixel = bitmapSource.Format.BitsPerPixel / 8;
            var palette = bitmapSource.Palette;

            // Cálculos de ajuste
            var adjustedIconSize = ICON_LARGE;
            var adjustedDpi = DEFAULT_DPI;
            var screenScaling = (double)currentImageSize / adjustedIconSize;
            if (screenScaling < 1.0) screenScaling = 1.0;

            // Copiar dados dos pixels
            var stride = currentImageSize * bytesPerPixel;
            var arraySize = stride * currentImageSize;
            var pixelData = new byte[arraySize];
            bitmapSource.CopyPixels(pixelData, stride, 0);

            // Aplicar escalonamento
            var scaledSize = (int)(adjustedIconSize * screenScaling);
            var scaledDpi = (int)(adjustedDpi * screenScaling);

            var scaledBitmap = BitmapSource.Create(
                scaledSize,
                scaledSize,
                scaledDpi,
                scaledDpi,
                imageFormat,
                palette,
                pixelData,
                stride);

            scaledBitmap.Freeze(); // Melhorar desempenho

            return scaledBitmap;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar ícone: {ex.Message}");
            return null;
        }
    }
}