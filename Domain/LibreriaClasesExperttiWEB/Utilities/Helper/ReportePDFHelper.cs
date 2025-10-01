using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class ReportePDFHelper
    {

        public async Task<byte[]> GenerarReportePdf(int IdReporting, List<string> paramList, int IDDatosDeEmpresa, IConfiguration myConnectionString)
        {
            string Adjunto = string.Empty;

            var objReportings = new CatalogoDeReportingService();
            var objReportingsD = new CatalogoReportingServicesRepository(myConnectionString);

            string FilePath = string.Empty;
            FileStream FS = null;
            byte[] pdfBytes = null;
            try
            {
                objReportings = objReportingsD.Buscar(IdReporting);
                if (!string.IsNullOrEmpty(objReportings.NuevoPath))
                {
                    objReportings.Path = objReportings.NuevoPath;
                    objReportings.Url = objReportings.NuevoUrl;
                    paramList.Add("IDE="+ IDDatosDeEmpresa.ToString());
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
            finally
            {
                // rptReporte.Dispose();
            }

            return pdfBytes;
        }

    }
        public class CustomReportCredentials : IReportServerCredentials
    {
        private readonly string _userName;
        private readonly string _password;

        public CustomReportCredentials(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public WindowsIdentity ImpersonationUser => null;

        public ICredentials NetworkCredentials => new NetworkCredential(_userName, _password);

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;
            return false;
        }
    }

}
