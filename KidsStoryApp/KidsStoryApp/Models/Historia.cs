namespace KidsStoryApp.Models;

public class Historia
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;

    // Contenido generado
    public string ImagenUri { get; set; } = string.Empty; // URL o path local a la imagen
    public string AudioUri { get; set; } = string.Empty;  // URL o path local al audio
    public string Texto { get; set; } = string.Empty;     // Texto completo del cuento

    // Metadatos
    public int DuracionMin { get; set; }
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
}