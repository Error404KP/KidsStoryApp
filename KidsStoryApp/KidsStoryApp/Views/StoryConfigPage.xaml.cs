using KidsStoryApp.ViewModels;

namespace KidsStoryApp.Views;

public partial class StoryConfigPage : ContentPage
{
    public StoryConfigPage(StoryConfigViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}