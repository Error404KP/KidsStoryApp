using KidsStoryApp.ViewModels;

namespace KidsStoryApp.Views;

public partial class WelcomePage : ContentPage
{
    // Inyección de Dependencias: Recibimos el ViewModel automáticamente
    public WelcomePage(WelcomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}