using KidsStoryApp.Models;

namespace KidsStoryApp.ViewModels;

// [QueryProperty] permite recibir datos de la navegación
[QueryProperty(nameof(Historia), "Historia")]
public class PlayerViewModel : BaseViewModel
{
    private Historia _historia;
    public Historia Historia
    {
        get => _historia;
        set => SetProperty(ref _historia, value);
    }
}