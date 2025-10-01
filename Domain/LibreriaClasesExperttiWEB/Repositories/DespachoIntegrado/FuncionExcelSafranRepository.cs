using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using System.Net.Mail;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class FuncionExcelSafranRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public FuncionExcelSafranRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable CargaReporteJCJRPorPedimento(int MiIDDatosDeEmpresa, string MiReferencia)
        {
            DataTable dtb = new DataTable();

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand
                        {
                            Connection = cn,
                            CommandText = "NET_LAYOUT_SALIDAS_JC_JR_POR_REFERENCIA",
                            CommandType = CommandType.StoredProcedure
                        }
                    };

                    // @IDE INT 
                    SqlParameter param = dap.SelectCommand.Parameters.Add("@IDE", SqlDbType.Int, 4);
                    param.Value = MiIDDatosDeEmpresa;

                    // @NUM_REFE VARCHAR(14)
                    param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    param.Value = MiReferencia;

                    dap.Fill(dtb);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_LAYOUT_SALIDAS_JC_JR_POR_REFERENCIA");
                }
                finally
                {
                    cn.Close();
                    cn.Dispose();
                }
            }

            return dtb;
        }
        public bool GeneraLineaDeComandosEnvioDesftp(string MyBat, string MyFile)
        {
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cn.ConnectionString = SConexion;
                    cn.Open();

                    // Asigno el Stored Procedure
                    cmd.CommandText = $"EXECUTE NET_ENVIO_SFTP '{MyBat}', '{MyFile}'";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;

                    // Ejecuto el sp
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    cmd.Parameters.Clear();
                    cn.Close();
                }
            }

            return true;
        }

        public void DataTableJCJRToExcel(DataTable pDataTable, string MiPedimento,
                                   string MiRuta, List<string> lstCorreos,
                                   string MiReferencia, int Completo,
                                   int SoloFtp)
        {
            string vFileName;
            string MisDocumentos = MiRuta; // Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            vFileName = MisDocumentos;

            if (!Directory.Exists(vFileName))
            {
                Directory.CreateDirectory(vFileName);
            }

            try
            {
                vFileName = Path.Combine(vFileName, MiPedimento + ".tmp");

                using (StreamWriter writer = new StreamWriter(vFileName))
                {
                    // Write header
                    string sb = string.Join("\t", pDataTable.Columns.Cast<DataColumn>().Select(dc => dc.Caption));
                    writer.WriteLine(sb);

                    // Write rows
                    foreach (DataRow dr in pDataTable.Rows)
                    {
                        sb = string.Empty;
                        for (int i = 0; i < pDataTable.Columns.Count; i++)
                        {
                            if (!dr.IsNull(i))
                            {
                                sb += dr[i].ToString() + "\t";
                            }
                            else
                            {
                                sb += "\t";
                            }
                        }

                        writer.WriteLine(sb);
                    }
                }

                TextToExcelJCJR(vFileName, lstCorreos, MiReferencia, Completo, SoloFtp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void TextToExcelJCJR(string pFileName, List<string> lstCorreos,
                              string MiReferencia, int Completo,
                              int SoloFtp)
        {
            Microsoft.Office.Interop.Excel.XlRangeAutoFormat vFormato;
            Microsoft.Office.Interop.Excel.Application exc = new Microsoft.Office.Interop.Excel.Application();
            exc.Workbooks.OpenText(pFileName, Missing.Value, Missing.Value, Missing.Value,
                                    Microsoft.Office.Interop.Excel.XlTextQualifier.xlTextQualifierNone,
                                    Missing.Value, true);

            Microsoft.Office.Interop.Excel.Workbook wb = exc.ActiveWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;
            int valor = 1;
            var objHelp = new Helper(); // Ensure this class is defined

            try
            {
                if (valor > -1)
                {
                    switch (valor)
                    {
                        case 1:
                            vFormato = Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatSimple;
                            break;
                    }

                    var range = ws.Range[ws.Cells[1, 1], ws.Cells[ws.UsedRange.Rows.Count, ws.UsedRange.Columns.Count]];
                    range.AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatSimple);
                    range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    range.Font.Size = 8;
                    ws.Name = "Hoja1";
                    pFileName = pFileName.Replace("tmp", "xlsx");

                    // Delete the existing file if it exists
                    if (File.Exists(pFileName))
                    {
                        File.Delete(pFileName);
                    }

                    // Save the workbook
                    wb.SaveAs(Filename: pFileName,
                              FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook,
                              CreateBackup: false);
                }

                exc.Quit();
                ws = null;
                wb = null;
                exc = null;

                GC.Collect();

                if (valor > -1)
                {
                    var lstAdjuntos = new List<string> { pFileName };
                    Thread.Sleep(3000);

                    if (SoloFtp == 1)
                    {
                        var objBitaraJcJrData = new BitacoraDeEnviosJcJrRepository(_configuration);
                        objBitaraJcJrData.Insertar(MiReferencia, Completo);
                        GeneraLineaDeComandosEnvioDesftp("sftpJcJF.bat", pFileName);
                    }
                    else
                    {
                        if (EnviarCorreo("no-reply.mex@grupoei.com.mx", "", "", lstAdjuntos, lstCorreos))
                        {
                            var objBitaraJcJrData = new BitacoraDeEnviosJcJrRepository(_configuration);
                            objBitaraJcJrData.Insertar(MiReferencia, Completo);
                            GeneraLineaDeComandosEnvioDesftp("sftpJcJF.bat", pFileName);
                        }
                    }

                    lstAdjuntos.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EnviarCorreo(string lemailEnvio, string Asunto,
                         string CuerpoMsj, List<string> lstAdjunto,
                         List<string> Correos)
        {
            bool envio = false;
            try
            {
                var mail = new System.Net.Mail.MailMessage();
                var smtp = new SmtpClient();

                var objCuentasCorreoD = new CatalogoDeCuentasDeCorreoRepository(_configuration);
                var objCuentasCorreo = objCuentasCorreoD.BuscarPorId(5);
                if (objCuentasCorreo == null)
                {
                    throw new ArgumentException("No existe una cuenta configurada para el sistema expertti");
                }

                mail.Subject = Asunto;
                mail.Body = CuerpoMsj;
                mail.IsBodyHtml = true;
                mail.Bcc.Add(lemailEnvio);

                foreach (var item in Correos)
                {
                    mail.To.Add(item);
                }

                mail.From = new MailAddress(objCuentasCorreo.gEMail.Trim(), lemailEnvio);
                mail.Attachments.Clear();

                foreach (var item in lstAdjunto)
                {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(item));
                }

                mail.ReplyToList.Add(lemailEnvio); // Optional: allows you to add a Reply-To email address

                smtp.Host = objCuentasCorreo.gHost.Trim();
                smtp.Port = objCuentasCorreo.gPuertoMail;
                smtp.EnableSsl = objCuentasCorreo.EnableSsl;

                if (!Convert.ToBoolean(objCuentasCorreo.Relay))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(objCuentasCorreo.gEMail.Trim(), objCuentasCorreo.gPassMail.Trim());
                }

                smtp.ServicePoint.MaxIdleTime = 2000;
                smtp.Send(mail);
                envio = true;
            }
            catch (Exception ex)
            {
                envio = false;
                throw new Exception("EnviarCorreo: " + ex.Message);
            }

            return envio;
        }


    }
}
