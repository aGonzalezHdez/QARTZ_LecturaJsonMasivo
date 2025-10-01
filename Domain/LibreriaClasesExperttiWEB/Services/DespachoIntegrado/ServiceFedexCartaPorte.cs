using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaFedex;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Services.DespachoIntegrado
{
    public class ServiceFedexCartaPorte
    {
        public IConfiguration _configuration;
        public ServiceFedexCartaPorte(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> POSTCartaPorteFedex(CartaPorteFedex data)
        {
            WebApisRepository webApisRepository = new WebApisRepository(_configuration);
            string url = webApisRepository.Buscar(6).URL;

            if (!_configuration.GetConnectionString("dbCASAEI").Contains("172.25.32.4"))//Ambiente productivo
            {

                url = "https://9ba9f61a-d0c7-47a7-aeee-c59fa45309e8.mock.pstmn.io";
                throw new Exception("En ambiente desarrollo se necesita simular la respuesta");//Descomentar url
            }
            
            
            HttpCliente cliente = new HttpCliente();
            string datos = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return await cliente.POSTValidaCertificado(url, "brokerage-info", datos);
        }

    }
}
