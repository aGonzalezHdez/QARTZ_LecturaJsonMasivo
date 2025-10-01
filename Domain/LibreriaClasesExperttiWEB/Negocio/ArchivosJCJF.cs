using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
namespace LibreriaClasesAPIExpertti.Negocio
{
    public class ArchivosJCJF
    {
        public IConfiguration _configuration;
        SoiaDataRepository soiaDataRepository;
        SaaioPedimeRepository saaioPedime;
        FuncionExcelSafranRepository safranRepository;
        BitacoraDeEnviosJcJrRepository bitacoraDeEnviosJcJrRepository;
        public ArchivosJCJF(IConfiguration configuration)
        {
            _configuration = configuration;
            soiaDataRepository = new SoiaDataRepository(_configuration);
            saaioPedime = new SaaioPedimeRepository(_configuration);
            safranRepository = new FuncionExcelSafranRepository(_configuration);
            bitacoraDeEnviosJcJrRepository = new BitacoraDeEnviosJcJrRepository(_configuration);
        }
        public string EnviaPedimentosAJCJFDesdeDODA(string archivosAIFA,int idDoda,int IdEmpresa)
        {
            string resultado = "";
            var referencias = soiaDataRepository.ReferenciasDoda(idDoda);

            foreach ( var referencia in referencias )
            {
                var objSaaioPedime = saaioPedime.Buscar(referencia);
                if(objSaaioPedime != null)
                {
                    string MiReferencia = objSaaioPedime.NUM_REFE;
                    string SYear = objSaaioPedime.FEC_PAGO.Year.ToString();
                    string MiPedimento = SYear.ToString().Substring(2, 2) + objSaaioPedime.ADU_DESP + objSaaioPedime.PAT_AGEN + objSaaioPedime.NUM_PEDI;
                    DataTable dtb = safranRepository.CargaReporteJCJRPorPedimento(IdEmpresa, MiReferencia);
                    if (dtb.Rows.Count>0)
                    {

                        DataTableJCJRToExcel(dtb, MiPedimento, archivosAIFA, null, MiReferencia, 1, 1);
                    }
                }
            }

            return resultado;
        }
        public void DataTableJCJRToExcel(DataTable pDataTable, string MiPedimento,
                                 string MiRuta, List<string> lstCorreos,
                                 string MiReferencia, int Completo, int SoloFtp)
        {
            string vFileName;
            string MisDocumentos = MiRuta;

            vFileName = MisDocumentos;

            if (!Directory.Exists(vFileName))
            {
                Directory.CreateDirectory(vFileName);
            }

            try
            {
                vFileName = Path.Combine(vFileName, MiPedimento + ".tmp");

                using (StreamWriter sw = new StreamWriter(vFileName))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataColumn dc in pDataTable.Columns)
                    {
                        sb.Append(dc.Caption + "\t");
                    }
                    sw.WriteLine(sb.ToString());

                    foreach (DataRow dr in pDataTable.Rows)
                    {
                        sb.Clear();
                        for (int i = 0; i < pDataTable.Columns.Count; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                sb.Append(Convert.ToString(dr[i]) + "\t");
                            }
                            else
                            {
                                sb.Append("\t");
                            }
                        }
                        sw.WriteLine(sb.ToString());
                    }
                }

                TextToExcelJCJR(vFileName, lstCorreos, MiReferencia, Completo, SoloFtp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void TextToExcelJCJR(string pFileName, List<string> lstCorreos, string MiReferencia, int Completo, int SoloFtp)
        {
            Excel.XlRangeAutoFormat vFormato;
            Excel.Application exc = new Excel.Application();
            exc.Workbooks.OpenText(pFileName, Type.Missing, Type.Missing, Type.Missing, Excel.XlTextQualifier.xlTextQualifierNone, Type.Missing, true);

            Excel.Workbook wb = exc.ActiveWorkbook;
            Excel.Worksheet ws = (Excel.Worksheet)wb.ActiveSheet;

            int valor = 1;

            try
            {
                if (valor > -1)
                {
                    switch (valor)
                    {
                        case 1:
                            vFormato = Excel.XlRangeAutoFormat.xlRangeAutoFormatSimple;
                            break;
                        default:
                            vFormato = Excel.XlRangeAutoFormat.xlRangeAutoFormatNone;
                            break;
                    }

                    ws.Range[ws.Cells[1, 1], ws.Cells[ws.UsedRange.Rows.Count, ws.UsedRange.Columns.Count]]
                        .AutoFormat(Excel.XlRangeAutoFormat.xlRangeAutoFormatSimple);
                    ws.Range[ws.Cells[1, 1], ws.Cells[ws.UsedRange.Rows.Count, ws.UsedRange.Columns.Count]]
                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    ws.Range[ws.Cells[1, 1], ws.Cells[ws.UsedRange.Rows.Count, ws.UsedRange.Columns.Count]]
                        .Font.Size = 8;

                    ApplyNumberFormat(ws, "K", "yyyy/MM/dd HH:mm:ss");
                    ApplyNumberFormat(ws, "M", "yyyy/MM/dd HH:mm:ss");
                    ApplyNumberFormat(ws, "N", "yyyy/MM/dd HH:mm:ss");
                    ApplyNumberFormat(ws, "R", "yyyy/MM/dd HH:mm:ss");

                    ws.Name = "Hoja1";
                    pFileName = pFileName.Replace("tmp", "xlsx");

                    if (File.Exists(pFileName))
                    {
                        File.Delete(pFileName);
                    }

                    wb.SaveAs(Filename: pFileName, FileFormat: Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup: false);
                }

                exc.Quit();

                ws = null;
                wb = null;
                exc = null;

                GC.Collect();

                if (valor > -1)
                {
                    var p = new System.Diagnostics.Process
                    {
                        EnableRaisingEvents = false
                    };

                    //Se utiliza cuando se envia correos
                    //var lstAdjuntos = new List<string> { pFileName };

                    //System.Threading.Thread.Sleep(3000);

                    if (SoloFtp == 1)
                    {

                        bitacoraDeEnviosJcJrRepository.Insertar(MiReferencia, Completo);
                        
                        //Comentar para desarrollo
                        safranRepository.GeneraLineaDeComandosEnvioDesftp("sftpJcJF.bat", pFileName );
                    }
                    else
                    {
                        //Ya que desde proceso que lo invoca por el momento se envía SoloFTP=1 no se agregó este proceso
                       /* if (EnviarCorreo("no-reply.mex@grupoei.com.mx", "", "", lstAdjuntos, lstCorreos, MyConnectionString))
                        {
                            var objBitaraJcJrData = new BitacoraDeEnviosJcJrData();
                            objBitaraJcJrData.Insertar(MiReferencia, Completo, MyConnectionString);
                            GeneraLineaDeComandosEnvioDesftp("sftpJcJF.bat", pFileName, MyConnectionString);
                        }*/
                    }
                    //Se utiliza cuando se envía correos
                    //lstAdjuntos.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void ApplyNumberFormat(Excel.Worksheet ws, string column, string format)
        {
            Excel.Range columnRange = (Excel.Range)ws.Columns[column];
            columnRange.NumberFormat = format;
        }

    }
}
