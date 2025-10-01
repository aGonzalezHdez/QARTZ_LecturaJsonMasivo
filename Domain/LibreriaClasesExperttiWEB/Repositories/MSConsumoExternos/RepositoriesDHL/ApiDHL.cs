using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL
{
  
    public class ApiDHL
    {
        public string Msg = string.Empty;
        public bool GeneroToken = false;
        public WebApis objAPi = new();
        public IConfiguration _configuration;


        public ApiDHL(IConfiguration configuration)
        {
            WebApisRepository objAPiD = new(configuration);
            objAPi = objAPiD.Buscar(18);
            _configuration = configuration;
        }

        public string getToken()
        {
            string vToken = string.Empty;
            string json = string.Empty;

            try
            {
                // Crear el objeto de autenticación
                JsonAuthentication objJsonAuthentication = new JsonAuthentication
                {
                    user = objAPi.Usuario.Trim(),
                    pwd = objAPi.Password.Trim(),
                    typeUser = "Provider"
                };

                this.GeneroToken = false;

                // Serializar el objeto a JSON
                json = Newtonsoft.Json.JsonConvert.SerializeObject(objJsonAuthentication);

                // Configurar el HttpClientHandler para manejar la validación del certificado
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Solo para desarrollo, no usar en producción
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(objAPi.URL + "Authentication/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Asegurar que se utiliza TLS 1.2
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    // URL para la solicitud de login
                    string url = "Login";

                    // Realizar la solicitud POST
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    // Verificar el estado de la respuesta
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        this.Msg = result;
                        this.GeneroToken = true;

                        // Deserializar la respuesta para obtener el token
                        TokenDHL objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenDHL>(result);
                        vToken = objRespuesta.payload;
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

            return vToken;
        }

        private static bool CertificateValidationCallBack(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if (
                          (certificate.Subject == certificate.Issuer) &&
                          (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }

        public JsonRepuestaDHL postDHL(string Json, string TokenDHL, string CAIA, String Metodo2)
        {
            string respuesta = "";
            JsonRepuestaDHL objResp = new();
            objResp.Json = Json;
            objResp.TokenDHL = TokenDHL;
            objResp.CAIA = CAIA;
            
            try
            {
                string metodo = "UploadInformation";


                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Solo para desarrollo, no usar en producción
                };
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(objAPi.URL + Metodo2);
                    client.DefaultRequestHeaders.Add("TokenIAS", TokenDHL);
                    client.DefaultRequestHeaders.Add("CAIA", CAIA);

                    // Asegurar que se utiliza TLS 1.2
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string url = string.Format(metodo);
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                    string result = response.Content.ReadAsStringAsync().Result;
                    var status = response.StatusCode;

                    this.Msg = result.ToString();
                    objResp.respuesta = Msg;
  
                    if (status == System.Net.HttpStatusCode.OK) { objResp.status = true;  }
                    else { objResp.status = false; }

                }
            }


            catch (Exception er)
            {
                this.Msg = er.Message;
                respuesta = er.Message.Trim();
            }
            return objResp;
        }

        public bool Bitacora(string respuesta, bool status, int idReferencia, string guiaHouse)
        {
            bool guardo = false;
            BitacoraApiDHLRepository objBitacoraD = new(_configuration);
            try
            {
                if (status == false)
                {
                    JsonRespuesta objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonRespuesta>(respuesta);

                    BitacoraApiDHL objBitacora = new();

                    objBitacora.Mensaje = objRespuesta.payload.errorDetail;
                    objBitacora.TieneError = objRespuesta.payload.hasErrors;
                    objBitacora.transferReceiptId = objRespuesta.payload.transferReceiptId;
                    objBitacora.IdReferencia = idReferencia;
                    objBitacora.GuiaHouse = guiaHouse;

                    objBitacoraD.Insertar(objBitacora);
                    guardo = true;
                }
                else
                {
                    try
                    {
                        if (respuesta == "")
                        {
                            respuesta = "WEB API DHL no regreso respuesta";
                            throw new ArgumentException("enviado y sin respuesta de ws DHL");

                        }

                        JsonRespuesta objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonRespuesta>(respuesta);

                        BitacoraApiDHL objBitacora = new();
                        objBitacora.Mensaje = objRespuesta.payload.errorDetail;
                        objBitacora.TieneError = objRespuesta.payload.hasErrors;
                        objBitacora.transferReceiptId = objRespuesta.payload.transferReceiptId;
                        objBitacora.IdReferencia = idReferencia;
                        objBitacora.GuiaHouse = guiaHouse;

                        objBitacoraD.Insertar(objBitacora);
                        guardo = true;
                    }
                    catch (Exception ex)
                    {
                        BitacoraApiDHL objBitacora = new();

                        objBitacora.Mensaje = ex.Message.Trim();
                        objBitacora.TieneError = true;
                        objBitacora.transferReceiptId = "";
                        objBitacora.IdReferencia = idReferencia;
                        objBitacora.GuiaHouse = guiaHouse;

                        objBitacoraD.Insertar(objBitacora);
                        guardo = true;
                    }
                }
            }
            catch (Exception ex1 )
            {

                BitacoraApiDHL objBitacora = new();

                objBitacora.Mensaje = ex1.ToString()  +  respuesta.Trim();
                objBitacora.TieneError = true;
                objBitacora.transferReceiptId = "";
                objBitacora.IdReferencia = idReferencia;
                objBitacora.GuiaHouse = guiaHouse;

                objBitacoraD.Insertar(objBitacora);
                guardo = true;
            }



            return guardo;
        }



    }
}
