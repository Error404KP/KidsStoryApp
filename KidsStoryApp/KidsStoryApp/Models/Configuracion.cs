namespace KidsStoryApp.Models;

public class Configuracion
{
    // Datos de entrada del usuario (Tema, edad, nombre)
    public string Tema { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string NombrePersonaje { get; set; } = string.Empty;

    // Preferencias de audio y lectura (para futuras fases)
    public string Voz { get; set; } = "DefaultFemale"; // Identificador de voz para Azure TTS
    public string Idioma { get; set; } = "es-ES";      // Idioma por defecto
    public double VelocidadLectura { get; set; } = 1.0; // 1.0 es velocidad normal
}