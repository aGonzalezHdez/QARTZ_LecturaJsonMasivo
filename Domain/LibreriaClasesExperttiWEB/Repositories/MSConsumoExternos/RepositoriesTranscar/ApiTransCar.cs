using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesTransCar;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesTranscar
{
    public class ApiTransCar
    {
        public string Msg = string.Empty;
        public bool GeneroToken = false;
        public WebApis objAPi = new();
        public IConfiguration _configuration;


        public ApiTransCar(IConfiguration configuration)
        {
            WebApisRepository objAPiD = new(configuration);
            objAPi = objAPiD.Buscar(42);
            _configuration = configuration;
        }

        public string getToken()
        {
            string vToken = string.Empty;
            string json = string.Empty;
            string sessionCookie = string.Empty;

            try
            {
                TransCar_Token objJsonAuthentication = new TransCar_Token
                {
                    UserName = objAPi.Usuario.Trim(),
                    Password = objAPi.Password.Trim(),
                    CompanyDB = "SBO_TRANSCAR"
                };

                this.GeneroToken = false;

                json = Newtonsoft.Json.JsonConvert.SerializeObject(objJsonAuthentication);

                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(objAPi.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string url = "Login";

                    HttpResponseMessage response = client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        this.Msg = result;
                        this.GeneroToken = true;

                        TransCar_Respuesta objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<TransCar_Respuesta>(result);
                        vToken = objRespuesta.SessionId.Trim();

                        if (response.Headers.Contains("Set-Cookie"))
                        {
                            IEnumerable<string> cookies = response.Headers.GetValues("Set-Cookie");
                            sessionCookie = string.Join(";", cookies);
                        }
                    }
                    else
                    {
                        this.Msg = response.Content.ReadAsStringAsync().Result;
                        this.GeneroToken = false;
                    }
                }
            }
            catch (HttpRequestException er)
            {
                this.Msg = er.Message;
                vToken = er.Message + " json: " + json;
            }

            return sessionCookie;
        }



        public string sendApiTranscar(string json,string sessionCookie)
        {
            string Error = string.Empty;
            try
            {
                // Configurar el HttpClientHandler para manejar la validación del certificado
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Solo para desarrollo, no usar en producción
                };
               // HttpClient client = new HttpClient();

                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(objAPi.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Cookie", sessionCookie);

                    // Asegurar que se utiliza TLS 1.2
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    // URL para la solicitud de login
                    string url = "U_TS_MERCANCIAS_CP";

                    // Realizar la solicitud POST
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    // Verificar el estado de la respuesta
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        this.Msg = result;
                        Error = "Enviado Correctamente";
    
                     
                    }
                    else
                    {
                        try
                        {
                              this.Msg = response.Content.ReadAsStringAsync().Result;
                              TransCar_Error objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<TransCar_Error>(Msg);

                               Error = objRespuesta.error.message.ToString().Trim();
                               this.GeneroToken = false;
                        }
                        catch (Exception ex)
                        {
                            Error = ex.Message.Trim();
                            
                        }

                   
                    }
                }
            }
            catch (HttpRequestException er)
            {
                this.Msg = er.Message;
                Error = Msg;
            }

            return Error;
        }
    }
}
