using System.Windows.Input;
using KidsStoryApp.Views; // Asegúrate de que este namespace exista o crearemos la carpeta Views luego

namespace KidsStoryApp.ViewModels;

public class WelcomeViewModel : BaseViewModel
{
    public ICommand IniciarCommand { get; }

    public WelcomeViewModel()
    {
        IniciarCommand = new Command(OnIniciar);
    }

    private async void OnIniciar()
    {
        // Navegar a la página de configuración
        // Usamos la ruta "StoryConfigPage" que definiremos en AppShell o MauiProgram
        await Shell.Current.GoToAsync(nameof(StoryConfigPage));
    }
}