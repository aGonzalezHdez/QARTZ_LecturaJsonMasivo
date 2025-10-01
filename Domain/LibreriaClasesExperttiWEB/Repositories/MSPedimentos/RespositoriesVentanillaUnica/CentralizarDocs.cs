using iTextSharp.text;
using iTextSharp.text.pdf;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica
{
    public class CentralizarDocs
    {
        public string SConexion { get; set; }
        public string SConexionGP { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CentralizarDocs(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            SConexionGP = _configuration.GetConnectionString("dbCASAEIGP")!;
        }
        public string fConvertirGuiaaPDF(string NumeroDeReferencia, string MisDocumentos)
        {
            //IL_000e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0014: Expected O, but got Unknown
            //IL_0014: Unknown result type (might be due to invalid IL or missing references)
            //IL_001b: Expected O, but got Unknown
            string empty = string.Empty;
            try
            {
                string empty2 = string.Empty;
                PathsDocumentos val = new PathsDocumentos();
                DocumentosRepository val2 = new DocumentosRepository(_configuration);
                val = val2.BuscarPath(NumeroDeReferencia, "C2");
                string empty3 = string.Empty;
                empty3 = val.pathArchivo + NumeroDeReferencia + "_C2_01.tif";
                if (!File.Exists(empty3))
                {
                    empty3 = val.pathArchivo + NumeroDeReferencia + "_C2_1.tif";
                    if (!File.Exists(empty3))
                    {
                        empty3 = val.pathArchivo + NumeroDeReferencia + "_C2_01.TIF";
                        if (!File.Exists(empty3))
                        {
                            empty3 = val.pathArchivo + NumeroDeReferencia + "_C2_1.TIF";
                            if (!File.Exists(empty3))
                            {
                                throw new ArgumentException("No existe imagen de la guia para enviar a Wec");
                            }
                        }
                    }
                }

                empty = MisDocumentos + Path.GetFileNameWithoutExtension(empty3.Trim()) + ".pdf";
                Document document = new Document(PageSize.LETTER, 72f, 72f, 72f, 72f);
                PdfWriter instance = PdfWriter.GetInstance(document, new FileStream(empty, FileMode.Create, FileAccess.Write, FileShare.None));
                instance.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                instance.SetDefaultColorspace(PdfName.COLORSPACE, PdfName.DEFAULTGRAY);
                document.Open();
                document.NewPage();
                iTextSharp.text.Image instance2 = iTextSharp.text.Image.GetInstance(empty3.Trim());
                instance2.ScaleToFit(500f, 600f);
                instance2.SetDpi(300, 300);
                document.Add(instance2);
                instance.Flush();
                document.Close();
                instance = null;
                document = null;
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                Exception ex2 = ex;
                throw new ArgumentException("fConvertirGuiaaPDF:" + ex2.Message.Trim());
            }

            return empty;
        }

        public  bool ExisteDocumento(string archivo)
        {
            bool existe;

            if (File.Exists(archivo))
            {
                existe = true;
            }
            else
            {
                existe = false;
            }

            return existe;
        }

        public string SellarGuia(int idConsolMaster, string archivoGuia)
        {
            string archivoNuevo = string.Empty;
            try
            {
                archivoNuevo = fSellarGuia(idConsolMaster, archivoGuia);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return archivoNuevo;
        }

        private string fSellarGuia(int idConsolMaster, string archivoGuia)
        {
            string archivoNuevo = string.Empty;

            try
            {
                var objMasterD = new CatalogodeMasterRepository(_configuration);
                var objMaster = objMasterD.Buscar(idConsolMaster);

                if (objMaster == null)
                    throw new ArgumentException("No existe la guía Master");

                if (!objMaster.ImagenMasterizacion)
                    throw new ArgumentException("No existe sello de master");

                if (!objMaster.ImagenRevalidacion)
                    throw new ArgumentException("No existe sello de Revalidación");

                var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                var objUbicacion = objUbicacionD.Buscar(38);

                archivoNuevo = Path.Combine(
                    objUbicacion.Ubicacion.Trim(),
                    objMaster.FechaCaptura.Month.ToString("00") + objMaster.FechaCaptura.Year.ToString("0000"),
                    objMaster.GuiaMaster.Trim() + ".pdf"
                );

                if (!File.Exists(archivoNuevo))
                {
                    try
                    {
                        string archivoAnterior = Path.Combine(
                            objUbicacion.UbicacionAnterior.Trim(),
                            objMaster.FechaCaptura.Month.ToString("00") + objMaster.FechaCaptura.Year.ToString("0000"),
                            objMaster.GuiaMaster.Trim() + ".pdf"
                        );

                        File.Copy(archivoAnterior, archivoNuevo);
                    }
                    catch
                    {
                        // Silenciar el error como en el código original
                    }
                }

                string archivoMaster = Path.Combine(
                    objUbicacion.Ubicacion.Trim(),
                    objMaster.FechaArribo.Month.ToString("00") + objMaster.FechaArribo.Year.ToString("0000"),
                    objMaster.GuiaMaster.Trim() + "_M.png"
                );

                string archivoReval = Path.Combine(
                    objUbicacion.Ubicacion.Trim(),
                    objMaster.FechaArribo.Month.ToString("00") + objMaster.FechaArribo.Year.ToString("0000"),
                    objMaster.GuiaMaster.Trim() + "_V.png"
                );

                var documento = new iTextSharp.text.Document(PageSize.LETTER, 72, 72, 72, 72);
                PdfWriter pdfw = PdfWriter.GetInstance(documento, new FileStream(archivoNuevo, FileMode.Create, FileAccess.Write, FileShare.None));
                pdfw.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                pdfw.SetDefaultColorspace(PdfName.COLORSPACE, PdfName.DEFAULTGRAY);

                documento.Open();
                documento.NewPage();

                var imagen = iTextSharp.text.Image.GetInstance(archivoGuia.Trim());
                imagen.ScaleToFit(500.0f, 600.0f);
                imagen.SetDpi(300, 300);
                documento.Add(imagen);

                var imagen2 = iTextSharp.text.Image.GetInstance(archivoMaster.Trim());
                imagen2.SetAbsolutePosition(10, 200);
                imagen2.ScaleToFit(161.0f, 91.5f);
                imagen2.SetDpi(300, 300);
                documento.Add(imagen2);

                var imagen3 = iTextSharp.text.Image.GetInstance(archivoReval.Trim());
                imagen3.SetAbsolutePosition(220, 100);
                imagen3.ScaleToFit(214.5f, 122.0f);
                imagen3.SetDpi(300, 300);
                documento.Add(imagen3);

                pdfw.Flush();
                documento.Close();

                pdfw = null;
                documento = null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("fSellarGuia " + ex.Message.Trim());
            }

            return archivoNuevo;
        }

        public byte[] DescargarDocumento(string archivo)
        {
            byte[] arch = null;
            arch = File.ReadAllBytes(archivo);
            return arch;
        }
    }
}
