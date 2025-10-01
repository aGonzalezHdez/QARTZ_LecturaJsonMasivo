using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos.Interface;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
//using Gma.QrCodeNet.Encoding;
using System.Drawing;
using System.Drawing.Imaging;
//using Gma.QrCodeNet.Encoding.Windows.Render;
using KeepAutomation.Barcode.Bean;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class ImpresiondePedimentosRepository : IImpresiondePedimentosRepository
    {
        public string SConexion { get; set; }
        string IImpresiondePedimentosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;

        public ImpresiondePedimentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public void creaPDF417Pedimento(string NumeroDeReferencia, int Regla3140)
        {

            // Primero:	Ejecuta el SP que genera un archivo de entrada para la generación del Código de Barras
            // Segundo:	Genera el código de Barras
            // Tercero:	Actualiza la imagen

            // Primera parte
            DataTable dtb = new();
            string? Datos = "";
            string MyWorkingDir;
            int I;
            bool primerLinea = true;

            using (SqlConnection con = new(SConexion))
            using (SqlCommand cmd = new("NET_IMPRIME_CODIGODEBARRAS_PEDIMENTO", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NumeroDeReferencia;
                    cmd.Parameters.Add("@Imprime3_1_40", SqlDbType.Int).Value = Regla3140;


                    using (SqlDataAdapter dap = new(cmd))
                    {
                        dap.Fill(dtb);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al ejecutar el procedimiento almacenado: " + ex.Message);
                }
            }

            if ((dtb.Rows.Count == 1))
            {
                for (I = 0; I <= 11; I++)
                {                    
                    Datos += (!primerLinea ? Environment.NewLine : "") + dtb.Rows[0][I].ToString();
                    primerLinea = false;
                }

                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objUbicacion = objUbicacionDeArchivosRepository.Buscar(62);
                MyWorkingDir = (objUbicacion.Ubicacion);
            
                string filePath = Path.Combine(MyWorkingDir, "Pedimento.txt");
               
                // Crear y abrir el archivo para escribir
                using (StreamWriter file = new(filePath, false))              
                {
                    // Escribir los datos en el archivo
                    file.WriteLine(Datos);
                }

                // Segunda parte
                //BarCode pdf417 = new();

                //pdf417.Symbology = KeepAutomation.Barcode.Symbology.PDF417;
                //pdf417.CodeToEncode = File.ReadAllText(MyWorkingDir + "Pedimento.txt");
                //pdf417.PDF417RowCount = 20;
                //pdf417.X = 4;
                //pdf417.BarCodeWidth = 320;
                //pdf417.ImageFormat = ImageFormat.Png;
                //pdf417.generateBarcodeToImageFile(MyWorkingDir + "Pedimento.png");

                // Tercera parte: Generar el Código de Barras
                BarCode pdf417 = new()
                {
                    Symbology = KeepAutomation.Barcode.Symbology.PDF417,
                    CodeToEncode = File.ReadAllText(filePath),
                    PDF417RowCount = 20,
                    X = 4,
                    BarCodeWidth = 320,
                    ImageFormat = ImageFormat.Png
                };

                string imagePath = Path.Combine(MyWorkingDir, "Pedimento.png");
                pdf417.generateBarcodeToImageFile(imagePath);

                //using SqlConnection con = new(SConexion);
                //using SqlCommand cmd = new("NET_INSET_PEDIMENTOPDF417", con)
                //{
                //    CommandType = CommandType.StoredProcedure
                //};  
                //cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 13).Value = Referencia;
                //cmd.Parameters.Add("@RUTA", SqlDbType.VarChar, 200).Value = MyWorkingDir + "Pedimento.png";

                //try
                //{
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //}
                //catch (Exception ex)
                //{
                //    throw new ApplicationException("Error al guardar la imagen en la base de datos.", ex);
                //}

                // Guardar la imagen en la base de datos
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSET_PEDIMENTOPDF417", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 13).Value = NumeroDeReferencia;
                    cmd.Parameters.Add("@RUTA", SqlDbType.VarChar, 200).Value = imagePath;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new ApplicationException("Error al guardar la imagen en la base de datos.", ex);
                    }
                }
            }
            else
            {
                //MsgBox("No hay datos para generar el Código de Barras.");
            }
        }

        //public void CreaCodeQR(string NumeroDeReferencia)
        //{
        //    ReferenciasRepository objReferenciasData = new(_configuration);

        //    try
        //    {
        //        Referencias objReferencias = objReferenciasData.Buscar(NumeroDeReferencia);
        //        if (objReferencias != null)
        //        {
        //            string link = "http://172.24.32.58:91/Catalogos/Clientes/InicioCW";

        //            byte[] Img;
        //            // Crear el generador de códigos QR
        //            QrEncoder qrEncoder = new();
        //            QrCode qrCode = new();

        //            // Intentar codificar el enlace en el código QR
        //            if (qrEncoder.TryEncode(link, out qrCode))
        //            {
        //                // Crear un renderizador de gráficos
        //                GraphicsRenderer renderer = new(new FixedCodeSize(600, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

        //                // Usar un MemoryStream para almacenar la imagen
        //                using (MemoryStream ms = new())
        //                {
        //                    // Renderizar el código QR en el MemoryStream
        //                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

        //                    // Crear la imagen a partir del MemoryStream
        //                    using (Bitmap imagetemporal = new(ms))
        //                    {
        //                        // Redimensionar la imagen a 500x500
        //                        using (Bitmap image = new(imagetemporal, new Size(500, 500)))
        //                        {
        //                            // Convertir la imagen a un array de bytes
        //                            ImageConverter converter = new();
        //                            Img = (byte[])converter.ConvertTo(image, typeof(byte[]));

        //                            // Aquí puedes hacer lo que necesites con el array de bytes


        //                            using (SqlConnection con = new(SConexion))
        //                            {
        //                                try
        //                                {
        //                                    using (var cmd = new SqlCommand("NET_INSERT_IMG_CODEQR_PEDIMENTO", con))
        //                                    {
        //                                        con.Open();
        //                                        cmd.CommandType = CommandType.StoredProcedure;

        //                                        cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NumeroDeReferencia;
        //                                        cmd.Parameters.Add("@Img", SqlDbType.Image).Value = Img;

        //                                        cmd.ExecuteNonQuery();
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    throw new Exception(ex.Message.ToString());
        //                                }
        //                            }

        //                        }
        //                    }
        //                } 
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new Exception(MsgBox(ex));
        //    }
        //}

        public void CreaCodeQR(string NumeroDeReferencia)
        {
            ReferenciasRepository objReferenciasData = new(_configuration);
            string link = "http://172.24.32.58:91/Catalogos/Clientes/InicioCW"; // Definir la URL una sola vez

            try
            {             
                Referencias objReferencias = objReferenciasData.Buscar(NumeroDeReferencia);
                if (objReferencias == null)
                {                   
                    return; // Salir si no hay referencias
                }

                // Crear el generador de códigos QR y la imagen
                byte[]? img = GenerarCodigoQR(link);
                if (img == null)
                {                    
                    return;
                }

                // Guardar la imagen en la base de datos
                GuardarImagenEnBaseDeDatos(NumeroDeReferencia, img);
            }
            catch (Exception ex)
            {            
                throw new ApplicationException("Error al crear el código QR.", ex);
            }
        }

        private byte[]? GenerarCodigoQR(string link)
        {
            try
            {
                // Crear el generador de códigos QR
                QrEncoder qrEncoder = new();
                if (!qrEncoder.TryEncode(link, out QrCode qrCode))
                {
                    return null; // No se pudo codificar
                }

                // Crear un renderizador de gráficos
                GraphicsRenderer renderer = new(new FixedCodeSize(600, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

                // Usar un MemoryStream para almacenar la imagen
                using MemoryStream ms = new();
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

                // Redimensionar la imagen a 500x500
                using Bitmap imagetemporal = new(ms);
                using Bitmap image = new(imagetemporal, new Size(500, 500));

                // Convertir la imagen a un array de bytes
                ImageConverter converter = new();
                return (byte[])converter.ConvertTo(image, typeof(byte[]));
            }
            catch (Exception ex)
            {                
                throw new ApplicationException("Error al generar el código QR.", ex);
            }
        }

        private void GuardarImagenEnBaseDeDatos(string numeroDeReferencia, byte[] img)
        {
            using SqlConnection con = new(SConexion);
            using SqlCommand cmd = new("NET_INSERT_IMG_CODEQR_PEDIMENTO", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = numeroDeReferencia;
            cmd.Parameters.Add("@Img", SqlDbType.Image).Value = img;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {               
                throw new ApplicationException("Error al guardar la imagen en la base de datos.", ex);
            }
        }

        public async Task<string> GeneraPedimentoPDfCompleto(GenerarPedimento objGenerarPedimento)
        {
            //string NumeroDeReferencia = objGenerarPedimento.NumeroDeReferencia.ToString();
            int Regla3140 = objGenerarPedimento.Regla3140;

            //creaPDF417Pedimento(NumeroDeReferencia, Regla3140);
            //CreaCodeQR(NumeroDeReferencia);

            clReportes objRpr = new(_configuration);
            string base64String = null;

            try
            {  
                //var paramList = new List<string> {
                //    "NUM_REFE="+ NumeroDeReferencia.ToString()
                //};
                //var bytes = await objRpr.GenerarReportePdf(94, paramList , 2);
                //base64String = Convert.ToBase64String(bytes);
                //paramList.Clear();
                
            }
            catch (Exception ex)
            {                
                throw new ArgumentException(ex.Message);
            }

            return base64String;
        }

        public async Task<string> GeneraPedimentoPDfSimplificado(GenerarPedimento objGenerarPedimento)
        {
            clReportes objRpr = new(_configuration);
            //string NumeroDeReferencia = objGenerarPedimento.NumeroDeReferencia.ToString();
            int Regla3140 = objGenerarPedimento.Regla3140;            
            string base64String = null;

            try
            {
                //var paramList = new List<string> {
                //    "NUM_REFE="+ NumeroDeReferencia.ToString()
                //};
                //var bytes = await objRpr.GenerarReportePdf(95, paramList, 2);
                //base64String = Convert.ToBase64String(bytes);
                //paramList.Clear();

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return base64String;
        }
    }
}
