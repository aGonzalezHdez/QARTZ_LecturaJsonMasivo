using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Services
{
    public class TokenService
    {

        public async Task<string> GetToken(string url, string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                // Establecer encabezados
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Crear el contenido del formulario con todos los campos (incluso los vacíos)
                var formData = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("grant_type", ""),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("scope", ""),
            new KeyValuePair<string, string>("client_id", ""),
            new KeyValuePair<string, string>("client_secret", "")
        });
                url = url + "/tokens";
                // Enviar solicitud POST
                var response = await httpClient.PostAsync(url, formData);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON
                    var tokenData = JsonSerializer.Deserialize<Models.TokenResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Retornar el token
                    return tokenData?.access_token ?? throw new Exception("Token no encontrado en la respuesta");
                }

                // Leer contenido de error para obtener más detalles
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener el token: {response.StatusCode}, Detalles: {errorContent}");
            }
        }

    }
}
