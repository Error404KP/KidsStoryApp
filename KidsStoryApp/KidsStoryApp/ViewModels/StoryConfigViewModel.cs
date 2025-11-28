using System.Windows.Input;
using KidsStoryApp.Models;
using KidsStoryApp.Services;
using KidsStoryApp.Views;

namespace KidsStoryApp.ViewModels;

public class StoryConfigViewModel : BaseViewModel
{
    private readonly ITextGenerationService _textService;

    // ---------------------------------------------------------
    // PROPIEDADES (Vinculadas a la Vista XAML)
    // ---------------------------------------------------------

    private string _tema = "";
    public string Tema
    {
        get => _tema;
        set => SetProperty(ref _tema, value);
    }

    private int _edad = 5;
    public int Edad
    {
        get => _edad;
        set => SetProperty(ref _edad, value);
    }

    private string _nombrePersonaje = "";
    public string NombrePersonaje
    {
        get => _nombrePersonaje;
        set => SetProperty(ref _nombrePersonaje, value);
    }

    public ICommand GenerarCuentoCommand { get; }

    // ---------------------------------------------------------
    // CONSTRUCTOR (Inyección de Dependencias)
    // ---------------------------------------------------------
    public StoryConfigViewModel(ITextGenerationService textService)
    {
        _textService = textService;
        GenerarCuentoCommand = new Command(async () => await OnGenerarCuento());
    }

    // ---------------------------------------------------------
    // LÓGICA DE GENERACIÓN
    // ---------------------------------------------------------
    private async Task OnGenerarCuento()
    {
        // 1. Evitar múltiples clics mientras procesa
        if (IsBusy) return;

        IsBusy = true; // Activa el spinner de carga en la UI

        try
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(Tema))
            {
                await Shell.Current.DisplayAlert("Falta información", "Por favor, escribe un tema para el cuento.", "OK");
                return;
            }

            // 2. Preparar el objeto de configuración con los datos del usuario
            var config = new Configuracion
            {
                Tema = this.Tema,
                Edad = this.Edad,
                NombrePersonaje = string.IsNullOrWhiteSpace(this.NombrePersonaje) ? "un amigo misterioso" : this.NombrePersonaje
            };

            // 3. Llamar al servicio (Azure o Mock) para obtener el texto
            // Nota: Esto puede tardar unos segundos
            var textoGenerado = await _textService.GenerateTextAsync(config);

            if (string.IsNullOrEmpty(textoGenerado))
            {
                throw new Exception("El servicio de generación no devolvió ningún texto.");
            }

            // 4. Crear el objeto Historia para pasar a la siguiente pantalla
            var nuevaHistoria = new Historia
            {
                Titulo = $"Cuento de {config.Tema}",
                Texto = textoGenerado,
                FechaGeneracion = DateTime.Now,
                // En el futuro aquí asignarías la imagen generada
                ImagenUri = "dotnet_bot.png"
            };

            // 5. Preparar los parámetros de navegación
            var navigationParameter = new Dictionary<string, object>
            {
                { "Historia", nuevaHistoria }
            };

            // 6. Navegar a la página del reproductor (PlayerPage)
            // IMPORTANTE: 'PlayerPage' debe estar registrada en AppShell.xaml.cs
            await Shell.Current.GoToAsync(nameof(PlayerPage), navigationParameter);
        }
        catch (Exception ex)
        {
            // Si algo falla, mostramos una alerta visible al usuario
            await Shell.Current.DisplayAlert("Error", $"Ocurrió un problema al crear el cuento: {ex.Message}", "OK");

            // También imprimimos en la consola de depuración
            System.Diagnostics.Debug.WriteLine($"❌ ERROR GENERANDO CUENTO: {ex}");
        }
        finally
        {
            IsBusy = false; // Desactiva el spinner siempre, haya error o no
        }
    }
}