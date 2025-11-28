using KidsStoryApp.Views;

namespace KidsStoryApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // ---------------------------------------------------------
        // 🚨 REGISTRO DE RUTAS DE NAVEGACIÓN
        // ---------------------------------------------------------
        // Sin esto, Shell.Current.GoToAsync("StoryConfigPage") fallará y cerrará la app.

        Routing.RegisterRoute(nameof(StoryConfigPage), typeof(StoryConfigPage));
        Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));
    }
}