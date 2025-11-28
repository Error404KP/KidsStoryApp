using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using KidsStoryApp.Models;
using Microsoft.Extensions.Configuration;

namespace KidsStoryApp.Services;

public class AzureTextGenerationService : ITextGenerationService
{
    private readonly HttpClient _http;
    private readonly string _endpoint;
    private readonly string _deploymentName;
    private string? _apiKey; // Se cargará desde SecureStorage

    // Inyección de dependencias del HttpClient y Configuración
    public AzureTextGenerationService(HttpClient http, IConfiguration config)
    {
        _http = http;
        // Leemos configuración NO sensible desde appsettings.json
        // Asegúrate de que las claves en tu JSON coincidan con estas ("Azure:OpenAI:TextEndpoint")
        _endpoint = config["Azure:OpenAI:TextEndpoint"] ?? "";
        _deploymentName = config["Azure:OpenAI:DeploymentName"] ?? "";

        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GenerateTextAsync(Configuracion config)
    {
        // 1. Cargar API Key de forma segura si no está cargada
        if (string.IsNullOrEmpty(_apiKey))
        {
            // Intentamos obtener la clave guardada en el dispositivo
            _apiKey = await SecureStorage.Default.GetAsync("OpenAIApiKey");

            if (string.IsNullOrEmpty(_apiKey))
            {
                // Si no hay clave, lanzamos error o devolvemos mensaje
                // Tip: Para pruebas rápidas, puedes configurar la clave manualmente en el App.xaml.cs al inicio
                throw new InvalidOperationException("API Key no encontrada en SecureStorage. Asegúrate de guardarla primero.");
            }
        }

        // 2. Construir el Prompt (la instrucción para la IA)
        var nombrePersonaje = string.IsNullOrWhiteSpace(config.NombrePersonaje)
                              ? "un personaje valiente" : config.NombrePersonaje;

        var prompt = $"Escribe un cuento infantil corto para un niño de {config.Edad} años. " +
                     $"El tema es: '{config.Tema}'. El protagonista es {nombrePersonaje}. " +
                     $"La historia debe tener inicio, nudo y desenlace feliz. Usa un lenguaje mágico y descriptivo.";

        // 3. Crear el cuerpo del mensaje (JSON)
        var requestPayload = new
        {
            messages = new[]
            {
                new { role = "system", content = "Eres un narrador de cuentos infantiles creativo y amable." },
                new { role = "user", content = prompt }
            },
            max_tokens = 800,
            temperature = 0.7
        };

        var jsonPayload = JsonSerializer.Serialize(requestPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // 4. Configurar la URL y Headers
        // Formato estándar de Azure OpenAI
        var url = $"{_endpoint}/openai/deployments/{_deploymentName}/chat/completions?api-version=2024-02-15-preview";

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };
        // Azure requiere la clave en el header 'api-key'
        request.Headers.Add("api-key", _apiKey);

        // 5. Enviar y procesar respuesta
        try
        {
            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(jsonResponse);

            // Navegamos el JSON de respuesta de Azure para sacar solo el texto
            var textoGenerado = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return textoGenerado ?? "No se pudo generar el texto.";
        }
        catch (Exception ex)
        {
            // Devolvemos el error para verlo en pantalla si algo falla
            return $"Error de conexión: {ex.Message}";
        }
    }
}