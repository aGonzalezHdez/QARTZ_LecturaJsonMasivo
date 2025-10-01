using System.Net;
using System.Text;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF
{
    public class ApiJCJFManifiestoRepository
    {
        public IConfiguration _configuration;
        public string sConexion { get; set; }
        public ApiJCJFManifiestoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<ResposeJCJFManifiesto> RequestCargarManifiestoJCJF(object objRequestArchivo)
        {
            WebApisRepository objApiD = new(_configuration);
            WebApis objApi = objApiD.Buscar(22);

            ResposeJCJFManifiesto objResponse = new();
            var request = (HttpWebRequest)WebRequest.Create(objApi.URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", objApi.Password);


            string body = Newtonsoft.Json.JsonConvert.SerializeObject(objRequestArchivo);
            var data = Encoding.UTF8.GetBytes(body);
            request.ContentLength = data.Length;          

            try
            {
                using (var stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    var status = response.StatusCode;                  

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = new StreamReader(response.GetResponseStream()))
                        {
                            string result = await responseStream.ReadToEndAsync();
                            objResponse = JsonConvert.DeserializeObject<ResposeJCJFManifiesto>(result) ?? new ResposeJCJFManifiesto();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        objResponse.Error = errorResponse.StatusDescription;
                    }
                }
                else
                {
                    objResponse.Error = $"Request fallido: {ex.Message}";
                }
            }
            return objResponse;
        }
    }
}
