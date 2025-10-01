using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class clReportes
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public clReportes(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }   
        public async Task<byte[]> GenerarReportePdf(int IdReporting, List<string> paramList, int IDDatosDeEmpresa)
        {     
            CatalogoReportingServicesRepository objReportingsD = new (_configuration);
            byte[] pdfBytes;
            try
            {
                CatalogoDeReportingService objReportings = objReportingsD.Buscar(IdReporting);
                if (!string.IsNullOrEmpty(objReportings.NuevoPath))
                {
                    objReportings.Path = objReportings.NuevoPath;
                    objReportings.Url = objReportings.NuevoUrl;
                    paramList.Add("IDE=" + IDDatosDeEmpresa.ToString());
                }

                paramList.Add("rs:Command=Render");
                paramList.Add("rs:Format=PDF");

                var handler = new HttpClientHandler()
                {
                    Credentials = new NetworkCredential(objReportings.Usuario, objReportings.Password)
                };
                var client = new HttpClient(handler);
                string url = objReportings.Url + "?" + $"{objReportings.Path}&{string.Join("&", paramList)}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    pdfBytes = await response.Content.ReadAsByteArrayAsync();                  
                }
                else
                {
                    throw new Exception("Ha ocurrido un error obteniendo pdf");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }    
            return pdfBytes;
        }
    }
}
