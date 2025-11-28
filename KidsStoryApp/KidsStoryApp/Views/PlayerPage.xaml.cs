using KidsStoryApp.ViewModels;

namespace KidsStoryApp.Views;

public partial class PlayerPage : ContentPage
{
    public PlayerPage(PlayerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}