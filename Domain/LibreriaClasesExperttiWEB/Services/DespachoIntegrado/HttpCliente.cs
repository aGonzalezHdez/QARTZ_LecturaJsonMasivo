using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LibreriaClasesAPIExpertti.Services.DespachoIntegrado
{
    public class HttpCliente
    {
        public async Task<T> POST<T>(string base_url,string metodo, string Json, Dictionary<string,string> headers=null)
        {
            try
            {
                HttpClient client = new HttpClient();
                
                client.BaseAddress = new Uri(base_url);

                if(headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                    
                }

                string url = string.Format(metodo);
                var response = await client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json"));
                var result = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode;

                if (status!=System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(response.RequestMessage.ToString());
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
        public async Task<string> POSTValidaCertificado(string base_url, string metodo, string Json,Dictionary<string,string> headers=null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                HttpClient client = new HttpClient();

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }

                }

                client.BaseAddress = new Uri(base_url);

                string url = string.Format(metodo);
                var response = await client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json"));
                var result = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode;

                return result;
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
        public async Task<T> GETWithParameter<T>(string base_url, string metodo, Dictionary<string,string> parametros,Dictionary<string,string> headers = null)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(base_url);

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }

                }

                string string_parametros = "";

                if (parametros != null)
                {
                    string_parametros += "?";
                    foreach (var item in parametros)
                    {
                        string_parametros += $"{item.Key}={item.Value},";
                    }
                    string_parametros.TrimEnd(',');
                }

                string url = string.Format(metodo+string_parametros);
                var response = await client.GetAsync(url);
                var result = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode;

                if (status != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(response.RequestMessage.ToString());
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }

        public static bool ValidateServerCertificate(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
