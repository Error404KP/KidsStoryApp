using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;

// Importamos los namespaces de tus carpetas
using KidsStoryApp.Services;
using KidsStoryApp.ViewModels;
using KidsStoryApp.Views;

namespace KidsStoryApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ---------------------------------------------------------
        // 1. CARGA DE CONFIGURACIÓN (appsettings.json)
        // ---------------------------------------------------------
        var config = GetConfiguration();
        builder.Configuration.AddConfiguration(config);

        // ---------------------------------------------------------
        // 2. REGISTRO DE SERVICIOS (LÓGICA)
        // ---------------------------------------------------------

        // Registra HttpClient (necesario si usas Azure en el futuro)
        builder.Services.AddHttpClient();

        // ⚠️ CAMBIO IMPORTANTE: Usamos el servicio MOCK (Gratis/Local) por ahora.
        // Cuando tengas tu API Key de Azure, comenta la línea 'Mock' y descomenta la 'Azure'.

        builder.Services.AddSingleton<ITextGenerationService, MockTextGenerationService>();
        // builder.Services.AddSingleton<ITextGenerationService, AzureTextGenerationService>();

        // ---------------------------------------------------------
        // 3. REGISTRO DE VISTAS Y VIEWMODELS (MVVM)
        // ---------------------------------------------------------

        // Pantalla de Bienvenida
        builder.Services.AddSingleton<WelcomeViewModel>();
        builder.Services.AddSingleton<WelcomePage>();

        // Pantalla de Configuración
        builder.Services.AddSingleton<StoryConfigViewModel>();
        builder.Services.AddSingleton<StoryConfigPage>();

        // Pantalla de Reproductor
        builder.Services.AddSingleton<PlayerViewModel>();
        builder.Services.AddSingleton<PlayerPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    // ---------------------------------------------------------
    // MÉTODO HELPER PARA CARGAR RECURSO INCRUSTADO
    // ---------------------------------------------------------
    private static IConfiguration GetConfiguration()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var builder = new ConfigurationBuilder();

        // Asegúrate de que 'KidsStoryApp' sea el namespace correcto de tu proyecto
        using var stream = assembly.GetManifestResourceStream("KidsStoryApp.appsettings.json");

        if (stream != null)
        {
            builder.AddJsonStream(stream);
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⚠️ ADVERTENCIA: No se encontró appsettings.json.");
        }

        return builder.Build();
    }
}