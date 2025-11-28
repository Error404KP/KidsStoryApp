using System.Threading.Tasks;
using KidsStoryApp.Models;

namespace KidsStoryApp.Services;

public class MockTextGenerationService : ITextGenerationService
{
    // Simula una espera de 2 segundos para parecer real
    public async Task<string> GenerateTextAsync(Configuracion config)
    {
        await Task.Delay(2000);

        // Creamos un cuento simple usando los datos del usuario
        string nombre = string.IsNullOrWhiteSpace(config.NombrePersonaje) ? "nuestro héroe" : config.NombrePersonaje;
        string tema = string.IsNullOrWhiteSpace(config.Tema) ? "una aventura misteriosa" : config.Tema;

        // Construimos el texto manualmente
        string cuento =
            $"Había una vez, en un mundo lejano, un niño de {config.Edad} años llamado {nombre}. " +
            $"Un día, {nombre} decidió salir en busca de {tema}. " +
            $"Caminó por bosques encantados y montañas de cristal. " +
            $"De repente, se encontró con un problema inesperado relacionado con {tema}, pero gracias a su valentía, " +
            $"logró resolverlo. Al final, {nombre} regresó a casa feliz y aprendió una gran lección. " +
            $"\n\n(Nota: Este es un cuento generado localmente sin IA para pruebas gratis).";

        return cuento;
    }
}