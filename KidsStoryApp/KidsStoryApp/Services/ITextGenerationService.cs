using KidsStoryApp.Models;

namespace KidsStoryApp.Services;

public interface ITextGenerationService
{
    // Método que recibe la configuración y devuelve el texto del cuento
    Task<string> GenerateTextAsync(Configuracion config);
}