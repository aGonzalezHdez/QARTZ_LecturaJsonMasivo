using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Text;
using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF
{
    public class ApiJCJFRepository
    {
        WebApis objApi = new WebApis();
        public string Msg;
        public bool Respuesta;

        public IConfiguration _configuration;
        public ApiJCJFRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            WebApisRepository objApiD = new WebApisRepository(_configuration);

            objApi = objApiD.Buscar(19);
        }
        public async Task<bool> Cliente(string vURL, string metodo, string Json)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(vURL);

                string url = string.Format(metodo);
                var response = await client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json"));
                var result = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode;

                if (status != System.Net.HttpStatusCode.OK)
                {
                    Msg = result;
                    JsonRespuesta objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonRespuesta>(result);

                    Msg = objRespuesta.Mensaje.Trim();
                    Respuesta = objRespuesta.pOk;

                    return false;
                }
                else
                {
                    JsonRespuesta objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonRespuesta>(result);

                    Msg = objRespuesta.Mensaje.Trim();
                    Respuesta = objRespuesta.pOk;


                    return true;
                }
            }
            catch (Exception er)
            {
                Msg = er.Message;
                return false;
            }
        }

        public async Task WSpostJCJF(string Metodo, string Json)
        {
            await Cliente(objApi.URL.Trim(), Metodo, Json);

            string respuesta = Msg;

            if (respuesta == "")
                throw new ArgumentException("enviado y sin respuesta de ws JCJF");
        }

    }
}
