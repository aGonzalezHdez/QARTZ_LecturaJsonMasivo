using Amazon.Runtime.Internal.Transform;
using Amazon.S3.Model;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL;

using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;

namespace LibreriaClasesAPIExpertti.Services.DespachoIntegrado
{
    public class ServiceDHLCartaPorte
    {
        public IConfiguration _configuration;
        public ServiceDHLCartaPorte( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Token> Token()
        {
            WebApisRepository webApisRepository = new WebApisRepository(_configuration);
            HttpCliente cliente = new HttpCliente();
            var url = webApisRepository.Buscar(1);

            clLogin login = new clLogin();
            login.user = url.Usuario;
            login.pwd = url.Password; 
            string baseurl = url.URL;
            if (_configuration.GetConnectionString("dbCASAEI").Contains("172.25.32.4"))
            {
                baseurl = baseurl + "Autenticacion";//concatenar Autenticacion a url 
            }
            else
            {
               // baseurl = "https://83361fe0-0372-4cf4-8fd9-8d007b4bb1f8.mock.pstmn.io/Autenticacion/";//concatenar Autenticacion a url 
                throw new Exception("En ambiente desarrollo se necesita simular la respuesta");//Descomentar baseurl
            }
            
            string datos = Newtonsoft.Json.JsonConvert.SerializeObject(login);


            return await cliente.POST<Token>(baseurl, "Login", datos);
        }
        public async Task<CPorteConsultaIs> POSTCartaPorteDHL(PredodaWS data)
        {
            WebApisRepository webApisRepository = new WebApisRepository(_configuration);

            string url = webApisRepository.Buscar(1).URL;
            if (_configuration.GetConnectionString("dbCASAEI").Contains("172.25.32.4"))//Ambiente productivo
            {
                url = url + "Predoda";
            }
            else
            {
                url = "https://83361fe0-0372-4cf4-8fd9-8d007b4bb1f8.mock.pstmn.io/Predoda/";
                throw new Exception("En ambiente desarrollo se necesita simular la respuesta");//Descomentar url
            }
                
            HttpCliente cliente = new HttpCliente();

            var token = await Token();

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"TokenCartaPorte", token.payload }
            };

            string datos = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return await cliente.POST<CPorteConsultaIs>(url,"Registro",datos,headers);
        }

        public async Task<CPorteConsultaIs> ConsultaProcesoId(int idpredoda)
        {
            WebApisRepository webApisRepository = new WebApisRepository(_configuration);

            string url = webApisRepository.Buscar(1).URL;

            if (_configuration.GetConnectionString("dbCASAEI").Contains("172.25.32.4"))//Ambiente productivo
            {
                url = url + "Predoda";
            }
            else
            {
                url = "https://83361fe0-0372-4cf4-8fd9-8d007b4bb1f8.mock.pstmn.io/Predoda/";
                throw new Exception("En ambiente desarrollo se necesita simular la respuesta");//Descomentar url
            }

            
           
            HttpCliente cliente = new HttpCliente();
            
            Dictionary<string,string> parametros = new Dictionary<string, string> {
                {"preDodaId",idpredoda.ToString()}
            };

            var token = await Token();

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"TokenCartaPorte", token.payload }
            };

            return await cliente.GETWithParameter< CPorteConsultaIs>(url, "ConsultaProcesoId", parametros, headers);
        }

        public async Task<UUIDCartaPorteDHL> ConsultaUUID(string procesoId)
        {
            WebApisRepository webApisRepository = new WebApisRepository(_configuration);

            string url = webApisRepository.Buscar(1).URL;
           

            if (_configuration.GetConnectionString("dbCASAEI").Contains("172.25.32.4"))//Ambiente productivo
            {
                url = url + "Predoda";
            }
            else
            {
                url = "https://83361fe0-0372-4cf4-8fd9-8d007b4bb1f8.mock.pstmn.io/Predoda/";
                throw new Exception("En ambiente desarrollo se necesita simular la respuesta");//Descomentar url
            }

                HttpCliente cliente = new HttpCliente();
            Dictionary<string, string> parametros = new Dictionary<string, string> {
                {"procesoId",procesoId}
            };

            var token = await Token();

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"TokenCartaPorte", token.payload }
            };

            return await cliente.GETWithParameter<UUIDCartaPorteDHL>(url, "Consulta", parametros, headers);
        }
    }
}
