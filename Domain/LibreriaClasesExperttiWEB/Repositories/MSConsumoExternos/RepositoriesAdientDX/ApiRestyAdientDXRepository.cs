using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.AdientDXSACI;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesAdientDX;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesAdientDXSACI
{
    public class ApiRestyAdientDXRepository : IApiRestyAdientDXRepository
    {
        public string SConexion { get; set; }
        string IApiRestyAdientDXRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public ApiRestyAdientDXRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<string> EnviarArchivosConJsonAsync(string uri, string bearerToken, string jsonData, List<string> archivos)
        {
            string message = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, uri);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    using var form = new MultipartFormDataContent();

                    // Agrega el JSON como parte del cuerpo
                    var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    form.Add(jsonContent, "data");

                    // Agrega los archivos
                    foreach (var archivo in archivos)
                    {
                        if (File.Exists(archivo))
                        {
                            var fileStream = File.OpenRead(archivo);
                            var fileContent = new StreamContent(fileStream);
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            form.Add(fileContent, "files", Path.GetFileName(archivo));
                        }
                        else
                        {
                            throw new FileNotFoundException("Archivo no encontrado: " + archivo);
                        }
                    }

                    request.Content = form;

                    // Enviar la solicitud
                    using var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        message = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        var errorDetails = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error HTTP {response.StatusCode}: {errorDetails}");
                    }
                }
                catch (Exception ex)
                {
                    // Puedes usar un logger aquí si es necesario
                    message = $"Error: {ex.Message}";
                }
            }

            return message;
        }


        public async Task<string> Inicio(AdientDXRequets objAdientDXRequets)
        {
            string? message;
            try
            {
                AdientDXApiRepository objAdientDXApiRepository = new(_configuration);
                AdientDXApi objAdientDXApi = objAdientDXApiRepository.Buscar()
                                     ?? throw new Exception("No se encontró configuración para AdientDXApi.");

                string? lUri = objAdientDXApi.Uri;
                string? lToken = objAdientDXApi.Token;
                string? lUserId = objAdientDXApi.UserId;
                string? lReceiver = objAdientDXApi.Receiver;

                string jsonData = $@"{{""company-id"":""{objAdientDXRequets.RFC}"",""user-id"":""{lUserId}"",""name"":""{objAdientDXRequets.Name}"",""receiver"":""{lReceiver}"",""file-count"": {objAdientDXRequets.ArchivosBase64?.Count ?? 0}}}";

                UbicaciondeArchivosRepository ubicaciondeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = ubicaciondeArchivosRepository.Buscar(180)
                    ?? throw new Exception("No se encontró configuración de ubicación para archivos.");

                if (!Directory.Exists(objRuta.Ubicacion))
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }

                var archivos = new List<string>();

                int contador = 1;
                foreach (var archivoBase64 in objAdientDXRequets.ArchivosBase64)
                {
                    try
                    {
                        Byte[] bytes = Convert.FromBase64String(archivoBase64.ContenidoBase64);
                        string nombreArchivo = Path.Combine(objRuta.Ubicacion + archivoBase64.Nombre);
                        System.IO.File.WriteAllBytes(nombreArchivo, bytes);

                        archivos.Add(nombreArchivo);
                        contador++;
                    }
                    catch (FormatException ex)
                    {
                        throw new Exception($"El archivo '{archivoBase64.Nombre}' no tiene un Base64 válido: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error al guardar el archivo '{archivoBase64.Nombre}': {ex.Message}");
                    }
                }

                message = await EnviarArchivosConJsonAsync(lUri, lToken, jsonData, archivos);

                BitacoraAdienDX objBitacoraAdienDX = new();
                objBitacoraAdienDX.Id = 0;
                objBitacoraAdienDX.NumerodeReferencia = objAdientDXRequets.NumeroDeReferencia;
                objBitacoraAdienDX.CompanyId = objAdientDXRequets.RFC;
                objBitacoraAdienDX.Menssaje = message;

                BitacoraAdientDX(objBitacoraAdienDX);


            }
            catch (Exception ex)
            {
                throw new Exception("Error en Inicio: " + ex.Message, ex);
            }

            return message;
        }

        public void BitacoraAdientDX(BitacoraAdienDX objBitacoraAdienDX)
        {
            BitacoraAdienDXRepository objBitacoraAdienDXData = new (_configuration);
            try
            {
                int id = objBitacoraAdienDXData.Insertar(objBitacoraAdienDX);
            }
            catch (Exception ex)
            {
                //MsgBox("Error: " + ex.Message(), MsgBoxStyle.OkOnly);
            }
        }

    }
}
