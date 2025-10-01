using Org.BouncyCastle.Utilities.IO;
using System.Data.SqlClient;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.WinForms;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Chilkat;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class ReportesAsync
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ReportesAsync(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<string> GenerarAcusesCove(int idReporting, int IdReferencia, string ConsFact, string Usuario)
        {

            string vArchivo = string.Empty;
            string NombreDeArchivo = "";


            string vPath = string.Empty;

            var objRefe = new Referencias();
            var objRefeD = new ReferenciasRepository(_configuration);
            objRefe = objRefeD.Buscar(IdReferencia);
            if (objRefe == null)
            {
                throw new ArgumentException("hubo un error en el id de la referencia");
            }

            var objCove = new FacturasCove();
            var objCoveD = new FacturasCoveRepository(_configuration);
            objCove = objCoveD.Buscar(IdReferencia, Int32.Parse(ConsFact));

            if (objCove == null)
            {
                throw new ArgumentException("no existe Cove para la factura seleccionada");
            }

            var paramList = new List<ReportParameter>();

            if (idReporting == 11 | idReporting == 31)
            {
                if (objCove.numeroAdenda.Trim() == "")
                {
                    NombreDeArchivo = "Acuse_" + objCove.NumeroDeCOVE.Trim() + ".pdf";
                }
                else
                {
                    NombreDeArchivo = "Acuse_" + objCove.numeroAdenda.Trim() + ".pdf";
                }

                paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("IDREFERENCIA", IdReferencia.ToString(), false));
            }
            else
            {
                NombreDeArchivo = @"\" + objCove.NumeroDeCOVE.Trim() + ".pdf";
                paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID_REFERENCIA", IdReferencia.ToString(), false));
                paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("NUM_REFERENCIA", objRefe.NumeroDeReferencia.Trim(), false));

            }

            paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CONS_FACT", ConsFact, false));


            string MisDocumentos = string.Empty;
            string filepath = string.Empty;
            var objUbicacion = new UbicaciondeArchivos();
            var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
            objUbicacion = objUbicacionD.Buscar(121);
            if (objUbicacion == null)
            {
                throw new ArgumentException("No existe ubicacion de archivos Id. 121, MisDOcumentos");
            }

            MisDocumentos = objUbicacion.Ubicacion + @"\" + Usuario.Trim();
            filepath = MisDocumentos + @"\ExperttiTmp"; // & NombreDeArchivo + ".pdf"
            if (Directory.Exists(filepath) == false)
            {
                Directory.CreateDirectory(filepath);
            }
            vArchivo = await GenerarReportePdf(idReporting, paramList, NombreDeArchivo, filepath);

            // End If

            return vArchivo;
        }
        public async Task<string> GenerarReportePdf(int IdReporting, List<ReportParameter> paramList, string lNombre, string PathPDF)
        {

            string Archivo = string.Empty;
            string vAdjunto = string.Empty;


            try
            {
                Archivo = await System.Threading.Tasks.Task.Run(() =>
                {
                    var objReportings = new CatalogoDeReportingService();
                    var objReportingsD = new CatalogoReportingServicesRepository(_configuration);

                    var rptReporte = new Microsoft.Reporting.WinForms.ReportViewer();

                    objReportings = objReportingsD.Buscar(IdReporting);

                    rptReporte.ProcessingMode = ProcessingMode.Remote;

                    rptReporte.ServerReport.ReportServerCredentials.NetworkCredentials = new NetworkCredential(objReportings.Usuario.Trim(), objReportings.Password.Trim());

                    string myurl = objReportings.Url;
                    rptReporte.ServerReport.ReportServerUrl = new Uri(myurl);

                    rptReporte.ServerReport.ReportPath = objReportings.Path;

                    ReportParameterInfoCollection pInfo;

                    rptReporte.ServerReport.SetParameters(paramList);
                    pInfo = rptReporte.ServerReport.GetParameters();
                    
                    string mimeType = null;
                    string encoding = null;
                    string extension = null;
                    Warning[] warnings = null;
                    string[] streamids = null;



                    if (Directory.Exists(PathPDF) == false)
                    {
                        Directory.CreateDirectory(PathPDF);
                    }

                    string filepath = PathPDF + lNombre;


                    byte[] bytes;
                    bytes = rptReporte.ServerReport.Render("PDF", default, out mimeType, out encoding, out extension, out streamids, out warnings);
                    FileStream fs = new FileStream(filepath, FileMode.Create);

                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    vAdjunto = filepath;
                    rptReporte.ServerReport.DisplayName = lNombre.Trim();
                    rptReporte.ShowParameterPrompts = true;

                    rptReporte.ServerReport.Refresh();

                    return vAdjunto;

                });
            }


            catch (Exception ex)
            {
                throw new ArgumentException("No fue posible generar el documento " + ex.Message);
            }
            return Archivo;
        }

        public async Task<string> GenerarAcusesCoveRemesas(int numReporte, int IdReferencia, int consFact, string Usuario)
        {
            string vArchivo = string.Empty;

            var objRefeD = new ReferenciasRepository(_configuration);
            var objRefe = objRefeD.Buscar(IdReferencia);

            if (objRefe == null)
                throw new ArgumentException("hubo un error en el id de la referencia");

            var objCoveD = new FacturasCoveRepository(_configuration);
            var objCove = objCoveD.Buscar(IdReferencia, consFact);

            if (objCove == null)
                throw new ArgumentException("no existe Cove para la factura seleccionada");

            string MisDocumentos = string.Empty;
            var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
            var objUbicacion = objUbicacionD.Buscar(121);

            if (objUbicacion == null)
                throw new ArgumentException("No existe ubicacion de archivos Id. 121, MisDOcumentos");

            MisDocumentos = Path.Combine(objUbicacion.Ubicacion, Usuario.Trim());

            if (!Directory.Exists(MisDocumentos))
                Directory.CreateDirectory(MisDocumentos);

            try
            {
                var paramList = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                string NombreDeArchivo = string.Empty;

                if (numReporte == 35)
                {
                    NombreDeArchivo = "Acuse_" + objCove.NumeroDeCOVE.Trim() + ".pdf";
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("NUM_REFERENCIA", objRefe.NumeroDeReferencia, false));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("CONS_FACT", consFact.ToString(), false));
                }
                else
                {
                    NombreDeArchivo = "Cove_" + objCove.NumeroDeCOVE.Trim() + ".pdf";
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("NUM_REFERENCIA", objRefe.NumeroDeReferencia, false));
                    paramList.Add(new Microsoft.Reporting.WinForms.ReportParameter("NUM_REM", consFact.ToString(), false));
                }

                vArchivo = await GenerarReportePdf(numReporte, paramList, NombreDeArchivo,  MisDocumentos);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return vArchivo;
        }

    }
}
