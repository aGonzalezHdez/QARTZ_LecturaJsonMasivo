using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNube;
//using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic;
using NPOI.Util;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
//using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class ControldeSalidasRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public ControldeEventosRepository eventosRepository;

        public ControldeSalidasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            eventosRepository = new ControldeEventosRepository(_configuration);
        }

        public void ValidarNube()
        {

        }

        public async void InsertarCheckpoint(ControldeEventos evento)
        {
            await eventosRepository.InsertarEventoAsync(evento);
        }

        public async Task<string> Procesar(ControldeSalidas controldeSalidas)
        {
            string result = "";
            try
            {

                // BUSCAR SI EL IDCLIENTE TIENE ASIGNADO EJECUTIVO 
                int IdEvento = 0;
                int IdCheckPoint = controldeSalidas.IdCheckPoint;
                int IDReferencia = controldeSalidas.IdReferencia;
                int idUsuarioAsignado = 0;
                string Observaciones = controldeSalidas.Observaciones;

                var objCheck = new CatalogodeCheckPoints();
                var objCheckD = new CatalogodeCheckPointsRepository(_configuration);
                if (IdCheckPoint == 0)
                {
                    throw new ArgumentException("No se ha enviado un checkpoint");
                }

                var objUsuarioRepository = new CatalogoDeUsuariosRepository(_configuration);
                var GObjUsuario = new CatalogoDeUsuarios();

                GObjUsuario = objUsuarioRepository.BuscarPorId(controldeSalidas.IdUsuario);

                if (GObjUsuario == null)
                {
                    throw new ArgumentException("El usuario no existe");
                }

                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(controldeSalidas.IdReferencia, GObjUsuario.IDDatosDeEmpresa);


                objCheck = objCheckD.BuscarId(IdCheckPoint, objRefe.IdOficina,objRefe.IDReferencia);


                // GUIA PRECAPTURADA PARA VALIDACIÓN Y PAGO
                if (IdCheckPoint == 524)
                {
                    var objCustomsAlertsD = new CargaManifiestosRepository(_configuration);

                    var objCMF = new CustomerMasterFile();
                    var objCMFD = new CustomerMasterFileRepository(_configuration);

                    // 'PREVALIDADOR

                    var asignarObj = new AsignarGuias(_configuration);

                    // INSERTAMOS CHECK ASIGNACION DE PEDIMENTO PARA TENER UN NUMERO DE PEDIMENTO PARA PODER VALIDAR 
                    var objCatCheck = new CatalogodeCheckPoints();
                    var objCatCheckD = new CatalogodeCheckPointsRepository(_configuration);
                    objCatCheck = await objCatCheckD.BuscarPorDepto(64, GObjUsuario.IdOficina, 54, objRefe.IDReferencia);

                    var objRespuestaValidacion = asignarObj.Validaciones(objCatCheck, objRefe, GObjUsuario.IdUsuario, false, Observaciones.Trim(), GObjUsuario.IDDatosDeEmpresa);


                    var ObjSaaioPedime = new SaaioPedimeRepository(_configuration);
                    if (ObjSaaioPedime.NETSABERSIEXISTEFIRELEC(objRefe.NumeroDeReferencia) == 1)
                    {
                        throw new ArgumentException("La Referencia ya cuenta con Firma ELectrónica");
                    }

                    var SAAIOPEDIMEDATA = new SaaioPedimeRepository(_configuration);
                    var SAAIOPEDIME = new SaaioPedime();

                    SAAIOPEDIME = SAAIOPEDIMEDATA.Buscar(objRefe.NumeroDeReferencia);
                    if (SAAIOPEDIME == null)
                    {
                        throw new ArgumentException("No existe el Pedimento");
                    }


                    var objBitacoraDePrevalData = new BitacoraDePrevalRepository(_configuration);
                    objBitacoraDePrevalData.Delete(objRefe.NumeroDeReferencia);

                    var ObjCatalogodePrevalRepository = new CatalogodePrevalRepository(_configuration);
                    ObjCatalogodePrevalRepository.Buscar(SAAIOPEDIME.NUM_PEDI.Trim(), objRefe.NumeroDeReferencia, GObjUsuario.IdUsuario, "");


                    var objloaddgvErrores = new BitacoraDePrevalRepository(_configuration);
                    var dtbErrores = new DataTable();
                    dtbErrores = objloaddgvErrores.LlenarPorReferenciaErroresGrid(objRefe.NumeroDeReferencia);

                    var results = dtbErrores.AsEnumerable().Where(row => row.Field<int>("TIPODEERROR") == 1).FirstOrDefault();

                    int iSinErrores = 0;
                    if (results is not null)
                    {
                        iSinErrores = 1;
                    }
                    else
                    {
                        iSinErrores = 0;
                    }

                    switch (iSinErrores)
                    {
                        case 1:
                            {
                                throw new ArgumentException("Pre-Validación finalizada con errores: " + results["MensajeError"]);
                            }

                        case 2:
                            {
                                throw new ArgumentException("Pre-Validación finalizada sin errores, pero con advertencias: " + results["MensajeError"]);
                            }
                    }

                    int riel = 0;

                    objCMF = objCMFD.Buscar(objRefe.NumeroDeReferencia);
                    if (objCMF == null)
                    {
                        var objCustomsAlerts = new CustomsAlerts();
                        objCustomsAlerts = objCustomsAlertsD.BuscarPorGuiaHouse(objRefe.NumeroDeReferencia, GObjUsuario.IDDatosDeEmpresa);

                        if (!(objCustomsAlerts == null))
                        {
                            riel = objCustomsAlerts.IdRielWEC;

                            if (objCustomsAlerts.Piezas >= 2)
                            {
                                throw new ArgumentException("Esta referencia no puede proceder a nube prepago, contiene " + objCustomsAlerts.Piezas + "  bultos y unicamente se permite 1 bulto.");
                            }
                        }
                    }

                    else
                    {
                        riel = objCMF.idRielWEC;

                        if (objCMF.Piezas >= 2)
                        {
                            throw new ArgumentException("Esta referencia no puede proceder a nube prepago, contiene " + objCMF.Piezas + "  bultos y unicamente se permite 1 bulto.");
                        }
                    }

                    if (riel == 0)
                    {
                        throw new ArgumentException("Esta referencia no tiene un riel asignado");
                    }
                    if (riel == 2)
                    {
                        // FAST MORNING
                        await GeneraEdocumentFactura(IDReferencia, objRefe, GObjUsuario);
                    }
                    else
                    {
                        bool insertaCheckPRepago = false;
                        // Nube 2.0
                        var nubePrepago = new NubePrepagoRepository(_configuration);
                        var objEventoD = new ControldeEventosRepository(_configuration);
                        var clienteNube = nubePrepago.BuscarDetalleCliente(objRefe.IDCliente);
                        if (!(clienteNube == null))
                        {
                            if (clienteNube.ValidaProducto)
                            {
                                var productoNube = nubePrepago.BuscarProductoCliente(objRefe.IDReferencia, objRefe.IDCliente);
                                if (productoNube == string.Empty)
                                {
                                    await GeneraEdocumentFactura(IDReferencia, objRefe, GObjUsuario);
                                    insertaCheckPRepago = true;
                                }
                            }

                            else
                            {
                                await GeneraEdocumentFactura(IDReferencia, objRefe, GObjUsuario);
                                insertaCheckPRepago = true;
                            }
                        }


                        if (insertaCheckPRepago == false)
                        {
                            throw new ArgumentException("Esta referencia no puede proceder a nube prepago");
                        }
                    }
                }


                if (objCheck.AsignacionAutomatica == false)
                {
                    var objCotizadoresporCliente = new CatalogoDeCotizadoresPorCliente();
                    var objCotizadoresporClienteD = new CatalogoDeCotizadoresPorClienteRepository(_configuration);
                    objCotizadoresporCliente = objCotizadoresporClienteD.Buscar(objRefe.IDCliente, objRefe.IdOficina);

                    if (!(objCotizadoresporCliente == null))
                    {
                        controldeSalidas.IdUsuario = objCotizadoresporCliente.IdUsuario;
                    }

                    idUsuarioAsignado = controldeSalidas.IdUsuario;
                }


                var objRespuesta = new AsignarGuiasRespuesta();
                var lEventos = new ControldeEventos(IdCheckPoint, IDReferencia, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
                var ctrlEvD = new ControldeEventosRepository(_configuration);
                objRespuesta = await ctrlEvD.InsertarEvento1(lEventos, GObjUsuario.IdDepartamento, objRefe.IdOficina, false, GObjUsuario.IDDatosDeEmpresa);

                var objAsignar = new AsignarGuias(_configuration);
                if (idUsuarioAsignado > 0)
                {
                    objRespuesta = await objAsignar.ReasignarGuia(GObjUsuario.IdUsuario, idUsuarioAsignado, IDReferencia);
                }

                if (objRespuesta.IdUsuarioAsignado != 0)
                {
                    var objUserAsig = new CatalogoDeUsuarios();
                    var objUserAsigD = new CatalogoDeUsuariosRepository(_configuration);
                    objUserAsig = objUserAsigD.BuscarPorId(objRespuesta.IdUsuarioAsignado);
                    result = objUserAsig.Nombre.Trim();
                }
                else if (objCheck.IdDepartamentoDestino != 0)
                {
                    if (objCheck.AsignacionAutomatica)
                    {
                        throw new ArgumentException("No existen usuarios configurados para turnar al area correspondiente");
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<bool> GeneraEdocumentFactura(int idReferencia, Referencias objRefe, CatalogoDeUsuarios GObjUsuario)
        {

            var asignaciondeGuiasData = new AsignacionDeGuiasRepository(_configuration);

            asignaciondeGuiasData.ValidaFastMorning(objRefe.NumeroDeReferencia);

            asignaciondeGuiasData.ValidaCove(objRefe.NumeroDeReferencia);

            var objDocumentosPorGuiaD = new DocumentosporGuiaRepository(_configuration);
            // ENVIAR FACTURA COMERCIAL
            if (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(idReferencia, 14, 1)))
            {
                var documentoavucem = objDocumentosPorGuiaD.Buscar(idReferencia, 14); // Ya me regresa el ultimo consecutivo

                if (documentoavucem.idDocumento == 0)
                {
                    throw new ArgumentException("No se encontro factura para enviar a VUCEM");
                }

                if (documentoavucem.Extension == ".TIF")
                {
                    try
                    {
                        await ConvertirFacturaEnPDF(objRefe.NumeroDeReferencia.Trim(), GObjUsuario.IDDatosDeEmpresa, GObjUsuario);

                        documentoavucem = objDocumentosPorGuiaD.Buscar(idReferencia, 14); // Ya me regresa el ultimo consecutivo

                        if (documentoavucem.idDocumento == 0)
                        {
                            throw new ArgumentException("No se encontro factura para enviar a VUCEM");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("No se pudo convertir de TIF a PDF");
                    }
                }
                var objDigitalizados = new DigitalizadosVucem();
                var objDigitalizadosD = new DigitalizadosVucemRepository(_configuration);

                // Factura
                objDigitalizados = objDigitalizadosD.Buscar(idReferencia, 68);
                if (objDigitalizados == null)
                {
                    var nubePrepago = new NubePrepagoRepository(_configuration);
                    var thFactura = new Thread(async () => await nubePrepago.GeneraEDocument(documentoavucem.idDocumento, objRefe.IDReferencia, false, objRefe, GObjUsuario));
                    thFactura.Start();
                }
            }


            return default;
        }
        public async Task ConvertirFacturaEnPDF(string Referencia, int IDDatosDeEmpresa, CatalogoDeUsuarios GObjUsuario)
        {
            try
            {
                var objFacturaTif = new Documento();
                var objFacturaTifD = new DocumentosRepository(_configuration);
                objFacturaTif = objFacturaTifD.Buscar(Referencia.Trim(), "M9", "01");
                if (objFacturaTif == null)
                {
                    return;
                }

                var objPath = new PathsDocumentos();
                objPath = objFacturaTifD.BuscarPath(Referencia.Trim(), "M9");


                string vArchivo;
                vArchivo = objFacturaTif.RutaFisica.Trim();

                if (File.Exists(vArchivo) == false)
                {
                    try
                    {
                        var objubicacion = new UbicaciondeArchivos();
                        var objubicacionD = new UbicaciondeArchivosRepository(_configuration);
                        objubicacion = objubicacionD.Buscar(4);
                        if (!(objubicacion == null))
                        {
                            string NombreArchivo = Path.GetFileName(vArchivo);
                            string RutaFisicaAnterior = objubicacion.UbicacionAnterior.Trim() + NombreArchivo;

                            File.Copy(RutaFisicaAnterior, vArchivo);

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }


                if (File.Exists(vArchivo))
                {
                    if (Path.GetExtension(vArchivo).ToUpper() == ".TIF")
                    {
                        string Destino = string.Empty;
                        Destino = ConvertitTiFFaPDF(vArchivo);

                        if (File.Exists(Destino))
                        {

                            var objRefe = new Referencias();
                            var objRefeD = new ReferenciasRepository(_configuration);
                            objRefe = objRefeD.Buscar(Referencia.ToString().Trim(), IDDatosDeEmpresa);
                            if (!(objRefe == null))
                            {
                                var objDocxGuia = new DocumentosporGuia();
                                var objDocxGuiaD = new DocumentosporGuiaRepository(_configuration);
                                objDocxGuiaD.Eliminar(int.Parse(objFacturaTif.IdDocumento));
                                try
                                {
                                    File.Delete(vArchivo);
                                }
                                catch (Exception ex)
                                {
                                }

                                int ID = 0;
                                var objCentralizar = new CentralizarDocsS3(_configuration);
                                ID = await objCentralizar.AgregarDocumentos(Destino, objRefe, 14, "", 0, GObjUsuario, false, "");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public bool Horizontal(string RutaTiff)
        {
            bool Horiz = false;
            try
            {
                var bm = new System.Drawing.Bitmap(RutaTiff);
                int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                // Dim cb As New iTextSharp.text.pdf.PdfContentByte()

                for (int k = 0, loopTo = total - 1; k <= loopTo; k++)
                {
                    if (Horiz == true)
                    {
                        continue;
                    }
                    bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);

                    if (img.Width > 1700)
                    {
                        Horiz = true;
                    }
                    else
                    {
                        Horiz = false;
                    }

                }
            }


            catch (Exception ex)
            {
                throw new ArgumentException(RutaTiff.ToString() + ex.Message);
            }

            return Horiz;
        }
        public string ConvertitTiFFaPDF(string RutaTiff)
        {
            string ArchivoNuevo = string.Empty;
            try
            {
                var objUbicacion = new UbicaciondeArchivos();
                var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                objUbicacion = objUbicacionD.Buscar(37);

                if (!(objUbicacion == null))
                {
                    ArchivoNuevo = objUbicacion.Ubicacion + DateTime.Now.ToString("yyyyMMdd") + @"\" + Path.GetFileNameWithoutExtension(RutaTiff) + ".pdf";

                    if (Directory.Exists(Path.GetDirectoryName(ArchivoNuevo)) == false)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(ArchivoNuevo));
                    }
                }

                var documento = new Document(PageSize.LETTER, 72, 72, 72, 72);
                if (Horizontal(RutaTiff))
                {
                    documento.SetPageSize(PageSize.LETTER.Rotate());
                }

                PdfWriter pdfw;

                pdfw = PdfWriter.GetInstance(documento, new FileStream(ArchivoNuevo, FileMode.Create, FileAccess.Write, FileShare.None));
                pdfw.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                pdfw.SetDefaultColorspace(PdfName.COLORSPACE, PdfName.DEFAULTGRAY);

                // Apertura del documento.
                documento.Open();

                // Agregamos una pagina.



                var bm = new System.Drawing.Bitmap(RutaTiff);
                int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                // Dim cb As New iTextSharp.text.pdf.PdfContentByte()

                double x = 50d;
                double y = 50d;
                for (int k = 0, loopTo = total - 1; k <= loopTo; k++)
                {
                    documento.NewPage();
                    bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);

                    img.ScalePercent(72.0f / img.DpiX * 80);

                    // img.SetAbsolutePosition(x, y)
                    // img.SetDpi(300, 300)

                    var objPdfTable = new PdfPTable(1);
                    objPdfTable.AddCell(img);

                    documento.Add(img);
                }

                pdfw.Flush();
                documento.Close();

                pdfw = default;
                documento = default;
            }

            catch (Exception ex)
            {
                throw new ArgumentException("ConvertitTiFFaPDF: " + ex.Message.Trim());
            }

            return ArchivoNuevo;
        }
        public void CambiarFaccionesT1(string Referencia)
        {
            try
            {
                Eliminar(Referencia, "MC");
                Eliminar(Referencia.Trim(), "XP");
                Eliminar(Referencia.Trim(), "ZC");
                Eliminar(Referencia.Trim(), "OV");
                ModificarFraccionesaT1(Referencia.Trim());
            }

            catch (Exception ex)
            {
                throw new Exception("Error al cambiar las fracciones de pedimento " + ex.Message.Trim());
            }
        }
        public string  CambiarFaccionesA1(string referencia, int idCliente, int IdUsuario,int DeseaAgrupar ,int CriterioAgrupacion)
        {
            var mensaje = String.Empty;

            try
            {
                var ObjSaaioPedime = new SaaioPedimeRepository(_configuration);
                var _bitacoraRepository = new BitacoraGeneralRepository(_configuration);
                var referenciasRepository = new ReferenciasRepository(_configuration);
                var pedimento = ObjSaaioPedime.Buscar(referencia);
                var objReferencia = referenciasRepository.Buscar(referencia);

                if (pedimento == null)
                {
                    throw new ArgumentException("No se encontró la referencia en el sistema de pedimentos");
                }

                if (objReferencia == null)
                {
                    throw new ArgumentException("No se encontró la referencia");
                }

                pedimento.CVE_PEDI = "A1";
                ObjSaaioPedime.Modificar(pedimento);

                if (DeseaAgrupar == 1)
                {
                   mensaje= Agrupar(CriterioAgrupacion, referencia, idCliente, IdUsuario, pedimento);
                }

                _bitacoraRepository.Insertar(new BitacoraGeneral
                {
                    IdReferencia = objReferencia.IDReferencia,
                    IdUsuario = IdUsuario,
                    Modulo = "Verificacion",
                    Descripcion = "Se cambia a A1"
                });
            }
            catch (Exception ex) { 
                return ex.Message;
            
            }

            return mensaje; 
            
            
        }
        private string Agrupar(int CriterioAgrupacion, string lNumeroDeReferencia, int MyIdCliente, int IdUsuario, SaaioPedime saaioPedime)
        {
            string mensaje = string.Empty;
            try
            {
                int metodo = CriterioAgrupacion;
                var GObjUsuario = new CatalogoDeUsuariosRepository(_configuration).BuscarPorId(IdUsuario);

                if (metodo == 0)
                {
                    throw new Exception("Imposible agrupar, primero debe determinar un método");
                }

                var pedimeData = new SaaioPedimeRepository(_configuration);
                var pedime = pedimeData.Buscar(lNumeroDeReferencia);

                var partidasFacturaData = new PartidasDePedimentoParaDataGridViewRepository(_configuration);
                List<PartidasDePedimentoParaDataGridView> partidasFactura;


                partidasFactura = partidasFacturaData.CargarVistaPreviaDeAgrupacion(
                    lNumeroDeReferencia,
                    CriterioAgrupacion,
                    1,
                    saaioPedime.CVE_IMPO,
                    Int32.Parse(saaioPedime.IMP_EXPO),
                    Decimal.Parse(saaioPedime.TIP_CAMB.ToString()),
                    MyIdCliente
                    //,GObjUsuario.IdOficina
                );


                if (pedime != null && !string.IsNullOrEmpty(pedime.CVE_PEDI))
                {
                    var plantillasData = new PlantillaDeIdentificadoresGeneralesRepository(_configuration);
                    plantillasData.Insertar(
                        lNumeroDeReferencia,
                        MyIdCliente,
                        GObjUsuario.IdOficina,
                        Convert.ToInt32(pedime.IMP_EXPO),
                        pedime.CVE_PEDI                        
                    );

                    var clientesData = new CatalogoDeClientesExperttiRepository(_configuration);
                    var cliente = clientesData.Buscar(MyIdCliente);

                    if (cliente != null)
                    {
                        var fpClienteData = new CatalogoDeFPPorClienteRepository(_configuration);
                        var fpCliente = fpClienteData.Buscar(
                            cliente.RFC,
                            pedime.CVE_PEDI,
                            Convert.ToInt32(pedime.IMP_EXPO)                            
                        );

                        if (fpCliente != null)
                        {
                            var fracciData = new SaaioFracciRepository(_configuration);
                            fracciData.UpdateFormasDePago(
                                lNumeroDeReferencia,
                                fpCliente.FPADV.ToString(),
                                fpCliente.FPIVA.ToString(),
                                MyIdCliente
                            );
                        }
                    }
                }
                mensaje = "El proceso de agrupación finalizó con éxito";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return mensaje;
        }

        protected void ModificarFraccionesaT1(string Referencia)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_FRACCION_A_T1";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            Helper objHelp = new Helper();
            // ,@NUM_REFE  varchar
            param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            param.Value = Referencia;

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.VarChar, 15);
            param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_FRACCION_A_T1");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
        }
        public bool Eliminar(string NumRefe, string CvePerm)
        {
            bool Elimino;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_DELETE_SAAIO_IDEFRA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NUM_REFE  varchar
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = NumRefe;

                // ,@CVE_PERM  varchar
                param = cmd.Parameters.Add("@CVE_PERM", SqlDbType.VarChar, 2);
                param.Value = CvePerm;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.VarChar, 15);
                param.Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Elimino = true;
            }
            catch (Exception ex)
            {
                Elimino = false;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_DELETE_SAAIO_IDEFRA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return Elimino;
        }
        public string GeneraProformaS3(int IdReferencia, string NumeroGuia, int IdUsuario)
        {
            string sArchivoFenerado;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_GENERA_PROFORMA_S3";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;

            // ,@idTipoDocumento  int
            param = cmd.Parameters.Add("@IDREFERENCIA", SqlDbType.Int, 4);
            param.Value = IdReferencia;

            param = cmd.Parameters.Add("@GUIAHOUSE", SqlDbType.VarChar, 15);
            param.Value = NumeroGuia;

            param = cmd.Parameters.Add("@IDUSUARIO", SqlDbType.Int, 4);
            param.Value = IdUsuario;

            param = cmd.Parameters.Add("@ARCHIVOGENERADO", SqlDbType.VarChar, 500);
            param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();

                sArchivoFenerado = cmd.Parameters["@ARCHIVOGENERADO"].Value.ToString();


                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                sArchivoFenerado = "";
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_GENERA_PROFORMA_S3");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return sArchivoFenerado;
        }



    }
}
