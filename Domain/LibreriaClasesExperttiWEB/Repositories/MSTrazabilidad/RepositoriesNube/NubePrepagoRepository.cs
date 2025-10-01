using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonWec;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesRobot;
using LibreriaClasesAPIExpertti.Services.Negocios;
using LibreriaClasesAPIExpertti.Services.S3;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using wsCentralizar;
using wsVentanillaUnica;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Services.Turnado;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesConsultasWsExternos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM;
using LibreriaClasesAPIExpertti.Services.VentanillaUnica;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using CatalogoDeUsuarios = LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNube
{


    public partial class NubePrepagoRepository
    {
        public string SConexion { get; set; }
        public string SConexionGP { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public NubePrepagoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            SConexionGP = _configuration.GetConnectionString("dbCASAEIGP")!;
        }
        public int RegenerarProformaInsertar(int IdReferencia, int IdUsuario, string Fecha)
        {

            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_NUBE_REGENERAR_PROFORMA_INSERT";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            @param.Value = IdReferencia;

            @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
            @param.Value = IdUsuario;

            @param = cmd.Parameters.Add("@Fecha", SqlDbType.Date);
            @param.Value = Fecha;

            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_NUBE_REGENERAR_PROFORMA_INSERT");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public string BuscarProductoCliente(int IdReferencia, int IdCliente)
        {


            var dtb = new DataTable();
            SqlParameter @param;
            string result = string.Empty;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_NUBE_PREPAGO_PRODUCTO_CLIENTE_VALIDO";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    @param = dap.SelectCommand.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                    @param.Value = IdReferencia;

                    @param = dap.SelectCommand.Parameters.Add("@IdCliente", SqlDbType.VarChar, 25);
                    @param.Value = IdCliente;

                    dap.Fill(dtb);
                    var lstCodigos = new List<string>();
                    if (dtb.Rows.Count > 0)
                    {
                        foreach (DataRow fila in dtb.Rows)
                            lstCodigos.Add(fila["CodigoDelProducto"].ToString());
                        result = string.Join(",", lstCodigos.ToArray());
                    }

                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();

                    cn.Dispose();
                    throw new Exception(ex.Message.ToString());
                }

            }
            return result;

        }

        public DetalleConfCliente BuscarDetalleCliente(int IdCliente)
        {
            var detalleCliente = new DetalleConfCliente();

            SqlDataReader dataReader;

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_NUBE_PREPAGO_SEARCH_CLIENTES";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@IdCliente", SqlDbType.VarChar, 25);
                    @param.Value = IdCliente;

                    dap.Fill(dtb);

                    if (dtb.Rows.Count > 0)
                    {
                        foreach (DataRow fila in dtb.Rows)
                        {

                            detalleCliente.IdRow = Convert.ToInt32(fila["Id"]);
                            detalleCliente.IdCliente = Convert.ToInt32(fila["IdCliente"]);
                            detalleCliente.ValidaProducto = Convert.ToBoolean(fila["ValidaProducto"]);
                            detalleCliente.Etiqueta = Convert.ToBoolean(fila["FotoEtiqueta"]);
                        }
                    }
                    else
                    {
                        detalleCliente = default;
                    }



                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();

                    cn.Dispose();
                    throw new Exception(ex.Message.ToString());
                }

            }

            return detalleCliente;
        }
        public int BuscarConcecutivo(int IdDocumento, int IDReferencia)
        {
            int Consecutivo;
            try
            {
                var objDigitalizados = new DigitalizadosVucem();
                var objDigitalizadosD = new DigitalizadosVucemRepository(_configuration);

                objDigitalizados = objDigitalizadosD.Buscar(IDReferencia, IdDocumento);
                if (objDigitalizados == null)
                {
                    Consecutivo = 1;
                }
                else
                {
                    Consecutivo = objDigitalizados.Consecutivo + 1;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Buscar Concecutivo:" + ex.Message);
            }
            return Consecutivo;

        }
        public string RutadeDigitalizados(CatalogodeDocumentosVuce objDocumentosVuce, int Consecutivo, string Extension, string NumeroDeReferencia)
        {
            string vArchivoVucem = string.Empty;

            var objRutas = new UbicaciondeArchivosRepository(_configuration);
            var Rutas = new UbicaciondeArchivos();
            Rutas = objRutas.Buscar(30);


            string vPath = Rutas.Ubicacion;
            vArchivoVucem = vPath + NumeroDeReferencia + "_" + objDocumentosVuce.NoOficial.ToString() + "_" + Consecutivo.ToString() + Extension;


            return vArchivoVucem;
        }
        public DigitalizadosVucem LlenarObjDigitalizados(int IdDocumento, int txtConsecutivo, int IDReferencia)
        {
            var objDigVuce = new DigitalizadosVucem();
            var objDigVuceD = new DigitalizadosVucemRepository(_configuration);

            objDigVuce.IDDocumento = IdDocumento;
            objDigVuce.IDReferencia = IDReferencia;
            objDigVuce.Consecutivo = txtConsecutivo;
            objDigVuce.eDocument = "";
            objDigVuce.numeroDeTramite = "";
            objDigVuce.NoOperacion = 0;
            objDigVuce.RFCSello = "";
            objDigVuce.ErrorArchivo = false;
            objDigVuce.EnviadoSAT = false;
            objDigVuce.NoHojas = 0;
            objDigVuce.Extension = ".pdf";

            return objDigVuce;
        }
        public async Task<int> Vucem(int idDocumento, Referencias refe, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {
            int Result = 0;
            try
            {
                var objDocumentosporGuia = new Documento();
                var objDocumentosporGuiaD = new DocumentosRepository(_configuration);
                objDocumentosporGuia = objDocumentosporGuiaD.BuscarIdDoc(idDocumento);

                if (objDocumentosporGuia == null)
                {
                    throw new ArgumentException("Ocurrio un problema al consultar el documento");
                }

                var objTiposdeDoc = new CatalogodeTiposDeDocumentos();
                var objTiposdeDocD = new CatalogodeTiposDeDocumentosRepository(_configuration);
                objTiposdeDoc = objTiposdeDocD.Buscar(int.Parse(objDocumentosporGuia.IdTipoDocumento));
                if (objTiposdeDoc == null)
                {
                    throw new ArgumentException("Ocurrio un problema con el tipo de documento");
                }

                var objRequisitosD = new RequisitosporNumerodeGuiaRepository(_configuration);
                objRequisitosD.Modificar(refe.IDReferencia, objTiposdeDoc.IDRequisitos, 2, GObjUsuario.IdUsuario);

                // End If
                var objDocumentosVuce = new CatalogodeDocumentosVuce();
                var objDocumentosVuceD = new CatalogodeDocumentosVuceRepository(_configuration);
                objDocumentosVuce = objDocumentosVuceD.BuscarId(objTiposdeDoc.IdDocumentoVuce);
                if (objDocumentosVuce == null)
                {
                    throw new ArgumentException("Ocurrio un problema, Este tipo de documento no esta asociado al catalogo de VUCEM ");
                }


                if (objDocumentosVuce.IdDocumento == 0)
                {
                    throw new ArgumentException("Este tipo de documento no esta asociado al catalogo de VUCEM");
                }

                var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                string DocumentoFisico = string.Empty;

                if (bool.Parse(objDocumentosporGuia.S3) == true)
                {
                    var objUbicacionS3 = new UbicaciondeArchivos();
                    objUbicacionS3 = objUbicacionD.Buscar(180);
                    if (objUbicacionS3 == null)
                    {
                        throw new ArgumentException("Ocurrio un problema con la ubicación de descarga del S3");
                    }

                    if (Directory.Exists(objUbicacionS3.Ubicacion.Trim()) == false)
                    {
                        throw new ArgumentException("No fue posible alcanzar la ruta de acceso :" + objUbicacionS3.Ubicacion.Trim());
                    }

                    var objCentralizarS3 = new CentralizarDocsS3(_configuration);
                    DocumentoFisico = objUbicacionS3.Ubicacion.Trim() + idDocumento.ToString() + ".pdf";
                    if (File.Exists(DocumentoFisico) == false)
                    {
                        string ok = string.Empty;
                        ok = await objCentralizarS3.DescargarS3(DocumentoFisico, objDocumentosporGuia.RutaS3.Trim(), "grupoei.documentos");
                        if (ok.ToUpper() != "OK")
                        {
                            throw new ArgumentException("No se pudo descargar de S3 el Documento Guia Revalidada Id: " + DocumentoFisico);
                        }

                    }
                }

                else
                {
                    string RutaFisica = objDocumentosporGuia.RutaFisica; // Me.dgvDocumentos.Rows(e.RowIndex).Cells("RutaFisica").Value().ToString

                    if (File.Exists(RutaFisica) == false)
                    {
                        try
                        {
                            string RutaFisicaAnterior = objDocumentosporGuia.RutaFisicaAnterior;
                            if (File.Exists(RutaFisicaAnterior))
                            {
                                File.Copy(RutaFisicaAnterior, RutaFisica);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message.Trim());
                        }
                    }
                    DocumentoFisico = RutaFisica;
                }
                int Consecutivo;
                Consecutivo = BuscarConcecutivo(objDocumentosVuce.IdDocumento, refe.IDReferencia);

                var objDigitalizados = new DigitalizadosVucem();
                var objDigitalizadosD = new DigitalizadosVucemRepository(_configuration);
                objDigitalizados = LlenarObjDigitalizados(objDocumentosVuce.IdDocumento, Consecutivo, refe.IDReferencia);

                string vArchivoVucem = RutadeDigitalizados(objDocumentosVuce, Consecutivo, objDigitalizados.Extension, refe.NumeroDeReferencia);

                string NuevoPdf = string.Empty;
                NuevoPdf = await Convertira300dpis(DocumentoFisico, idDocumento.ToString() + ".pdf", vArchivoVucem, GObjUsuario);

                int id;
                id = objDigitalizadosD.Insertar(objDigitalizados);
                Result = id;
                if (GObjUsuario.Oficina.ironPDF)
                {
                    if (File.Exists(NuevoPdf))
                    {
                        File.Move(NuevoPdf, vArchivoVucem);
                    }
                    else
                    {
                        throw new ArgumentException("Exception Occured");
                    }
                }
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }
            return Result;
        }
        //Pendientes
        public async Task GeneraEDocument(int idDocumento, int IDReferenciaE, bool Turna, Referencias refe, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {
            var eDocuments = new DigitalizadosRecibir();
            var objDigitalizar = new Digitalizacion(_configuration);


            var objRespuesta = new AsignarGuiasRespuesta();
            var objEventosDa = new ControldeEventosRepository(_configuration);
            UbicaciondeArchivos objUbicacion = new UbicaciondeArchivos();
                                                UbicaciondeArchivosRepository objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                                                objUbicacion = objUbicacionD.Buscar(121);
                                                if ((objUbicacion == null))
            {
                throw new ArgumentException("No existe ubicacion de archivos Id. 121, MisDOcumentos");
            }
                                                   

            var pMisDocumentos = objUbicacion.Ubicacion + @"\" + GObjUsuario.Usuario.Trim()+ @"\ExperttiTmp\";

            try
            {
                string obsCheck = "";

                var IdDigitalizadosVucem = await Vucem(idDocumento, refe, GObjUsuario);
                // Enviar
                eDocuments = await objDigitalizar.Digitalizar(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);
                if (!(eDocuments == null))
                {
                    Thread.Sleep(10000);
                    // Recibir
                    eDocuments = await objDigitalizar.RecuperarEdocuments(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);

                    // INTENTO DOS
                    if (eDocuments.Err == true)
                    {
                        Thread.Sleep(10000);
                        eDocuments = await objDigitalizar.RecuperarEdocuments(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);
                    }

                    // INTENTO TRES
                    if (eDocuments.Err == true)
                    {
                        Thread.Sleep(10000);
                        eDocuments = await objDigitalizar.RecuperarEdocuments(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);
                    }

                    if (Turna)
                    {
                        if (eDocuments.Err == true) // si tiene error el edocument, mandar a salidas
                        {
                            var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
                            objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, "Error al obtener eDocument", GObjUsuario.IDDatosDeEmpresa);
                        }

                        else
                        {
                            // GUIA ARRIBADA PARA PAGO'
                            var objEventoCo = new ControldeEventos(526, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
                            objRespuesta = await objEventosDa.InsertarEvento(objEventoCo, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, "", GObjUsuario.IDDatosDeEmpresa);
                        }
                    }
                }




                else if (Turna)
                {
                    // GUIA ARRIBADA PARA SALIDAS'
                    var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
                    objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, "Error al mandar a digitalizar a VUCEM, no hubo respuesta.", GObjUsuario.IDDatosDeEmpresa);


                }
            }
            catch (Exception ex)
            {
                if (Turna)
                {
                    // GUIA ARRIBADA PARA SALIDAS'
                    var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
                    objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, "Error al obtener eDocument " + ex.Message, GObjUsuario.IDDatosDeEmpresa);

                }

            }
        }
        public string BuscarDocumentoGuia(string txtReferencia, string pMisDocumentosParameter)
        {
            string ArchivoPDF = string.Empty;
            try
            {
              
                if (!Directory.Exists(pMisDocumentosParameter))
                {
                    Directory.CreateDirectory(pMisDocumentosParameter);
                }

                var objCentralizar = new CentralizarDocs(_configuration);
                ArchivoPDF = objCentralizar.fConvertirGuiaaPDF(txtReferencia, pMisDocumentosParameter);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }


            return ArchivoPDF;
        }

        public async Task<int> RevalidaWec(Referencias referencia, string pMisDocumentos, int IdUsuario, int IDDatosDeEmpresa, int IdDepartamento, int IdOficina, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario, string observacion)
        {
            int IdDocumento = 0;
            try
            {
                var GuiasD = new SaaioGuiasRepository(_configuration);

                var objGuiaHouse = new SaaioGuias();
                objGuiaHouse = GuiasD.Buscar(referencia.NumeroDeReferencia, "H");

                var GuiaHouse = objGuiaHouse.GUIA.Trim();

                string ContenidoBase64 = string.Empty;
                byte[] ArchivoByte = File.ReadAllBytes(BuscarDocumentoGuia(referencia.NumeroDeReferencia, pMisDocumentos));

                ContenidoBase64 = Convert.ToBase64String(ArchivoByte);

                var objGuiaMaster = new SaaioGuias();
                objGuiaMaster = GuiasD.Buscar(GuiaHouse.Trim(), "M");

                var obj = new RevalidadosWEC();
                obj.pHouse = GuiaHouse;
                obj.pMaster = Strings.Replace(objGuiaMaster.GUIA.Trim(), "-", "");
                obj.pUsuario_Id = "GEIREVALIDA";
                obj.pImagen64 = ContenidoBase64;


                var objRespuesta = new RevalidacionWECRespuesta();
                //var ws = new WebApiWec();
                var WebApiWecRepository = new WebApiWecRepository(_configuration);
                objRespuesta = await WebApiWecRepository.WSRevalidaWec(obj);

                if (objRespuesta == null)
                {
                    throw new ArgumentException("Hubo un error en la transmisión de la información");
                }



                if (objRespuesta.pPDFBase64.Trim() != "")
                {
                    File.WriteAllBytes(pMisDocumentos + @"\" + referencia.NumeroDeReferencia + "_GH.pdf", Convert.FromBase64String(objRespuesta.pPDFBase64));
                }
                else
                {
                    if (objRespuesta.pDomRESTMsg.Trim() == "Guía revalidada con anterioridad para otro agente aduanal.")
                    {
                        int IDNoLiberar = 0;
                        var objGuiasNoLiberarD = new GuiasqueNoseDebenLiberarRepository(_configuration);
                        bool Guardo = false;
                        var ObjUbicacionDeArchivos = new UbicaciondeArchivosRepository(_configuration);
                        string NombreAsignado = "";
                        var objGuiasNoLiberar = new GuiasQueNoseDebenLiberar();

                        objGuiasNoLiberar.IDNoLiberar = IDNoLiberar;
                        objGuiasNoLiberar.NumeroDeGuia = referencia.NumeroDeReferencia;
                        objGuiasNoLiberar.Motivo = "Guía revalidada con anterioridad para otro agente aduanal.";
                        objGuiasNoLiberar.Activa = true;
                        objGuiasNoLiberar.IdUsuarioAlta = IdUsuario;
                        objGuiasNoLiberar.Posicion = "";
                        objGuiasNoLiberar.TodaslasAreas = true;
                        objGuiasNoLiberar.IDDepartamento = 0;
                        objGuiasNoLiberar.IDUsuarioBaja = 0;

                        IDNoLiberar = objGuiasNoLiberarD.Insertar(objGuiasNoLiberar);
                        objGuiasNoLiberarD.GenerarArchivoITCE(IDNoLiberar, "B", IdUsuario, ObjUbicacionDeArchivos.BuscaUbicacionDeArchivos(162));
                        // Se adiciona asignación de deteneidas 
                        if (objGuiasNoLiberar.Activa == true)
                        {
                            int IdEvento = 0;
                            var objEventosD = new ControldeEventosRepository(_configuration);
                            var objRefeD = new ReferenciasRepository(_configuration);
                            var objRefe = new Referencias();
                            objRefe = objRefeD.Buscar(objGuiasNoLiberar.NumeroDeGuia, IDDatosDeEmpresa);
                            if (!(objRefe == null))
                            {
                                var objEventos = new ControldeEventos(41, objRefe.IDReferencia, IdUsuario, DateTime.MinValue);
                                var objRespuesta2 = new AsignarGuiasRespuesta();
                                objRespuesta2 = await objEventosD.InsertarEvento(objEventos, IdDepartamento, IdOficina, false, "Guía revalidada con anterioridad para otro agente aduanal. ", IDDatosDeEmpresa);
                            }

                        }
                    }
                    throw new ArgumentException(objRespuesta.pDomRESTMsg.Trim());

                }

                int Result;
                var objDocumentosporguia = new wsCentralizar.DocumentosPorGuia();
                var objCentralizar = new CentralizarS3sp(_configuration);
                IdDocumento = await objCentralizar.AgregarDocumentos(pMisDocumentos + @"\" + referencia.NumeroDeReferencia + "_GH.pdf", referencia.IDReferencia, 1166, "", 0, GObjUsuario, false);

                File.Delete(pMisDocumentos + @"\" + referencia.NumeroDeReferencia + "_GH.pdf");
            }

            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message)
            }

            return IdDocumento;

        }

        public List<wsCentralizar.DocumentosPorGuia> BuscarAll(int idReferencia, int IdTipoDocumento)
        {
            var listRespuestas = new List<wsCentralizar.DocumentosPorGuia>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_ALLDOCUMENTOSPORGUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4);
            @param.Value = idReferencia;

            @param = cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.Int, 4);
            @param.Value = IdTipoDocumento;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    var objDOCUMENTOSPORGUIA = new wsCentralizar.DocumentosPorGuia();
                    objDOCUMENTOSPORGUIA.idDocumento = Convert.ToInt32(dr["idDocumento"]);
                    objDOCUMENTOSPORGUIA.idTipoDocumento = Convert.ToInt32(dr["idTipoDocumento"]);
                    objDOCUMENTOSPORGUIA.idReferencia = Convert.ToInt32(dr["idReferencia"]);
                    objDOCUMENTOSPORGUIA.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    objDOCUMENTOSPORGUIA.RutaFecha = dr["RutaFecha"].ToString();
                    objDOCUMENTOSPORGUIA.Extension = dr["Extension"].ToString();
                    objDOCUMENTOSPORGUIA.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                    listRespuestas.Add(objDOCUMENTOSPORGUIA);
                }
                else
                {
                    listRespuestas = null;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return listRespuestas;
        }
        public bool esRelacion(SaaioPedime objpedi, string NumeroDeReferencia, string NumRem, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {
            bool Relacion = false;
            if (objpedi.TIP_PEDI == null)
            {
                Relacion = false;
            }
            else if (objpedi.TIP_PEDI == "C")
            {
                if (objpedi.FIR_REME == null)
                {
                    throw new ArgumentException("Es necesaria la firma de remesa antes de enviar a COVE");
                }
                if (GObjUsuario.IDDatosDeEmpresa == 2)
                {
                    if (GObjUsuario.Oficina.DHL)
                    {
                        var objSaaioFactGui = new SaaioFacGui();
                        var objSaaioFactGuiD = new SaaaioFacGuiRepository(_configuration);
                        objSaaioFactGui = objSaaioFactGuiD.VerificarExistaHouseEnRemesa(NumeroDeReferencia.Trim(), NumRem.Trim());
                        if (objSaaioFactGui == null)
                        {
                            throw new ArgumentException("No se puede enviar a cove la Remesa por que no se ha integrado una guia House");
                        }
                    }
                }
            }

            if (!(objpedi.FIR_REME == null))
            {
                var lstFactur = new List<SaaioFactur>();
                var objFacturD = new SaaioFacturRepository(_configuration);
                lstFactur = objFacturD.CargarRemesa(NumeroDeReferencia.Trim(), Convert.ToInt32(NumRem.Trim()));
                if (lstFactur == null)
                {
                    throw new ArgumentException("No hay facturas relacionadas");
                }

                if (lstFactur.Count > 1)
                {
                    Relacion = true;
                }
                else
                {
                    Relacion = false;
                }
            }
            else
            {
                Relacion = false;
            }

            return Relacion;
        }

        public async Task EnviarCove(Referencias objRefe, int MyCons_Fact, SaaioFactur SAAIO_FACTUR, string pMisDocumentos, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {


            var SaaioFacturData = new Factura(objRefe.CapturaenCasa);


            if (SAAIO_FACTUR.MON_FACT == "MXP" & SAAIO_FACTUR.EQU_DLLS.ToString("#,###.00000000") == "1.00000000")
            {
                throw new Exception("ERROR, CUANDO LA MONEDA ES MXP, LA EQUIVALENCIA NO PUEDE SER 1.00000000");
            }

            string vRuta = string.Empty;
            var objUbicacion = new UbicaciondeArchivos();
            var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
            objUbicacion = objUbicacionD.Buscar(22);

            if (objUbicacion == null)
            {
                throw new ArgumentException("No existe ruta para generar los XML es necesario reportar a sistemas");
            }

            vRuta = objUbicacion.Ubicacion.Trim() + @"ExperttiTmp\";

            if (Directory.Exists(vRuta) == false)
            {
                Directory.CreateDirectory(vRuta);
            }

            var objpedi = new SaaioPedime();
            var objpediD = new SaaioPedimeRepository(_configuration);
            objpedi = objpediD.Buscar(objRefe.NumeroDeReferencia);

            bool Relacion = esRelacion(objpedi, objRefe.NumeroDeReferencia, SAAIO_FACTUR.NUM_REM.ToString(), GObjUsuario);

            var objFacturD = new SaaioFacturRepository(_configuration);
            objFacturD.ModificarRelacion(objRefe.NumeroDeReferencia, MyCons_Fact, Relacion);


            // 'VAMOS

            var objComprobanteD = new ComprobanteService(_configuration);
            var objRecuperarCoves = new wsRespuestaCove.RespuestaPeticion();

            string edoc = string.Empty;
            int IdUsuarioAutoriza = 0;

            if (Relacion == true)
            {

                string FacturasSinGuias = "";
                if (objRefe.IDCliente == 9087118)
                {
                    var ObjSAAIO_FACTURD = new SaaioFacturRepository(_configuration);
                    FacturasSinGuias = ObjSAAIO_FACTURD.NET_SABER_SI_EXISTE_GUIA_EN_FACTURA_DE_REMESA(objRefe.NumeroDeReferencia, Convert.ToInt32(SAAIO_FACTUR.NUM_REM));
                }


                if (string.IsNullOrEmpty(FacturasSinGuias))
                {
                    objRecuperarCoves = await objComprobanteD.EnviarCoveNOIA(objRefe.NumeroDeReferencia, Convert.ToInt32(SAAIO_FACTUR.NUM_REM), edoc, true, vRuta, GObjUsuario.IDDatosDeEmpresa, pMisDocumentos, GObjUsuario, IdUsuarioAutoriza);
                }

                else
                {
                    throw new ArgumentException("En las siguientes facturas no fue declarado un número de guía house:" + FacturasSinGuias);

                }
            }
            else
            {

                objRecuperarCoves = await objComprobanteD.EnviarCove(objRefe.NumeroDeReferencia, MyCons_Fact, edoc, true, vRuta, GObjUsuario.IDDatosDeEmpresa, objRefe.PedimentoGlobal, pMisDocumentos, GObjUsuario, IdUsuarioAutoriza);

            }

        }

        public async Task GeneraEDocumentAll(int idTipo, int IDReferenciaE, bool Turna, Referencias refe, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario, string pMisDocumentos, bool envioCOVE)
        {
            var eDocuments = new DigitalizadosRecibir();
            var objDigitalizar = new Digitalizacion(_configuration);

            bool responseRecibirCove = true;
            var objRespuesta = new AsignarGuiasRespuesta();
            var objEventosDa = new ControldeEventosRepository(_configuration);

            try
            {
                string obsCheck = "";
                string obsCove = "Error al obtener COVE";
                var documentosavucem = BuscarAll(IDReferenciaE, idTipo);

                var listRespuestas = new List<bool>();

                if (envioCOVE)
                {
                    var SaaioFacturData = new Factura(refe.CapturaenCasa);
                    var MyCons_Fact = SaaioFacturData.EXTRAE_MAX_CONS_FACT(refe.NumeroDeReferencia);

                    var SAAIO_FACTUR = new SaaioFactur();

                    SAAIO_FACTUR = SaaioFacturData.Buscar(refe.NumeroDeReferencia, MyCons_Fact);

                    try
                    {
                        await EnviarCove(refe, MyCons_Fact, SAAIO_FACTUR, pMisDocumentos, GObjUsuario);
                    }
                    catch (Exception ex)
                    {
                        obsCheck = ex.Message;
                    }
                    Thread.Sleep(12000);

                    try
                    {
                        // INTENTO UNO

                        responseRecibirCove = await RecibirCoves(refe, GObjUsuario, pMisDocumentos);
                    }
                    catch (Exception ex)
                    {
                        obsCove = ex.Message;
                    }

                    if (responseRecibirCove == false)
                    {
                        // INTENTO DOS
                        Thread.Sleep(10000);

                        try
                        {
                            responseRecibirCove = await RecibirCoves(refe, GObjUsuario, pMisDocumentos);
                        }
                        catch (Exception ex)
                        {
                            obsCove = ex.Message;
                        }



                        if (responseRecibirCove == false)
                        {
                            // INTENTO TRES
                            Thread.Sleep(10000);
                            try
                            {

                                responseRecibirCove = await RecibirCoves(refe, GObjUsuario, pMisDocumentos);
                            }
                            catch (Exception ex)
                            {
                                obsCove = ex.Message;
                            }
                        }

                    }
                }

                foreach (wsCentralizar.DocumentosPorGuia itemDocumento in documentosavucem)
                {
                    var IdDigitalizadosVucem = await Vucem(itemDocumento.idDocumento, refe, GObjUsuario);
                    // Enviar
                    eDocuments = await objDigitalizar.Digitalizar(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);
                    if (!(eDocuments == null))
                    {
                        Thread.Sleep(10000);
                        // Recibir
                        eDocuments = await objDigitalizar.RecuperarEdocuments(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);

                        // INTENTO DOS
                        if (eDocuments.Err == true)
                        {
                            Thread.Sleep(10000);
                            eDocuments = await objDigitalizar.RecuperarEdocuments(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);
                        }

                        // INTENTO TRES
                        if (eDocuments.Err == true)
                        {
                            Thread.Sleep(10000);
                            eDocuments = await objDigitalizar.RecuperarEdocuments(IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);
                        }

                        if (eDocuments.Err == false)
                        {
                            listRespuestas.Add(true);
                        }
                        else
                        {
                            obsCheck = "Error al obtener eDocument";
                            listRespuestas.Add(false);
                        }
                    }

                    else
                    {
                        listRespuestas.Add(false);
                        obsCheck = "Error al mandar a digitalizar a VUCEM, no hubo respuesta.";
                    }
                }


                if (Turna)
                {
                    if (responseRecibirCove == false)
                    {
                        // GUIA ARRIBADA PARA SALIDAS'
                        var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.MinValue);
                        objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, obsCove, GObjUsuario.IDDatosDeEmpresa);
                    }
                    else if (listRespuestas.Count == 0)
                    {
                        // GUIA ARRIBADA PARA SALIDAS'
                        var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.MinValue);
                        objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, obsCheck, GObjUsuario.IDDatosDeEmpresa);
                    }

                    else if (listRespuestas.Contains(false))
                    {
                        // GUIA ARRIBADA PARA SALIDAS'
                        var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.MinValue);
                        objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, obsCheck, GObjUsuario.IDDatosDeEmpresa);
                    }

                    else
                    {
                        bool generacalculo = false;
                        var objPedimeD = new SaaioPedimeRepository(_configuration);
                        var objPedime = objPedimeD.Buscar(refe.NumeroDeReferencia);
                        if (!(objPedime == null))
                        {

                            var fechaActual = DateTime.Now;
                            var robotPago = new RobotPagoRepository(_configuration);
                            var result = robotPago.ValidateFirPago(refe.NumeroDeReferencia);
                            if (result.Rows.Count > 0)
                            {
                                if (result.Rows[0]["FEC_PAGO"] is DBNull)
                                {
                                    generacalculo = true;
                                }
                                else if (Convert.ToDateTime(result.Rows[0]["FEC_PAGO"]).Date < fechaActual.Date)
                                {
                                    generacalculo = true;
                                }
                            }

                        }
                        if (generacalculo)
                        {
                            var objHelp = new Helper();
                            await objHelp.CalcularImpuestos(refe.NumeroDeReferencia, SConexion);
                        }

                        // Threading.Thread.Sleep(5000)

                        // GUIA ARRIBADA PARA PAGO'
                        var objEventoCo = new ControldeEventos(526, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.MinValue);
                        objRespuesta = await objEventosDa.InsertarEvento(objEventoCo, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, "", GObjUsuario.IDDatosDeEmpresa);

                    }
                }
            }

            catch (Exception ex)
            {
                if (Turna)
                {
                    // GUIA ARRIBADA PARA SALIDAS'
                    var objEventoCoSalida = new ControldeEventos(527, IDReferenciaE, GObjUsuario.IdUsuario, DateTime.MinValue);
                    objRespuesta = await objEventosDa.InsertarEvento(objEventoCoSalida, GObjUsuario.IdDepartamento, GObjUsuario.IdOficina, false, "Error al obtener eDocument " + ex.Message, GObjUsuario.IDDatosDeEmpresa);

                }

            }
        }
        public async Task<bool> RecibirCoves(Referencias referencia, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario, string pMisDocumentos)
        {
            try
            {
                string vRuta = string.Empty;
                var objUbicacion = new UbicaciondeArchivos();
                var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                objUbicacion = objUbicacionD.Buscar(22);

                if (objUbicacion == null)
                {
                    throw new ArgumentException("No existe ruta para generar los XML es necesario reportar a sistemas");
                }

                vRuta = objUbicacion.Ubicacion.Trim() + @"ExperttiTmp\";

                string Archivo = vRuta + referencia.NumeroDeReferencia.Trim() + ".xml";

                var objComprobante = new ComprobanteService(_configuration);

                var objpedi = new SaaioPedime();
                var objpediD = new SaaioPedimeRepository(_configuration);
                objpedi = objpediD.Buscar(referencia.NumeroDeReferencia);

                var lstSaiioFact = new List<SaaioFactur>();
                var objSaiioFactD = new SaaioFacturExperttiRepository(_configuration);

                lstSaiioFact = objSaiioFactD.Cargar(referencia.NumeroDeReferencia);
                if (lstSaiioFact == null)
                {
                    throw new Exception("No existen facturas para esta operacion");
                }

                var listRespuestas = new List<bool>();


                var objGenerales = new ComponentesGenerales();
                var objDatos = new Datos(_configuration);
                int IdUsuarioAutoriza = 0;

                foreach (SaaioFactur itemFact in lstSaiioFact)
                {
                    var objFactCove = new FacturasCove();
                    var objFactCoveD = new FacturasCoveRepository(_configuration);
                    objFactCove = objFactCoveD.Buscar(referencia.IDReferencia, itemFact.CONS_FACT);
                    bool Relacion = esRelacion(objpedi, referencia.NumeroDeReferencia, itemFact.NUM_REM.ToString(), GObjUsuario);
                    if (!(objFactCove == null))
                    {
                        objGenerales = objDatos.DatosGenerales(referencia.NumeroDeReferencia, GObjUsuario.IDDatosDeEmpresa);                        
                        wsRespuestaCove.RespuestaPeticion objRespuestas;

                        if (Relacion == true)
                        {
                            objRespuestas = await objComprobante.RecuperarCOVENOIA(objGenerales.Sello, objFactCove.NumeroDeOperacion.ToString(), referencia, Archivo, GObjUsuario, IdUsuarioAutoriza, pMisDocumentos);
                        }
                        else
                        {
                            objRespuestas = await objComprobante.RecuperarCOVE(objGenerales.Sello, objFactCove.NumeroDeOperacion.ToString(), referencia, Archivo, pMisDocumentos, GObjUsuario, IdUsuarioAutoriza);
                        }

                        bool respuestaPositiva = false;

                        string eDocument = "";
                        if (!(objRespuestas.respuestasOperaciones == null))
                        {
                            foreach (wsRespuestaCove.RespuestaOperacion item in objRespuestas.respuestasOperaciones)
                            {
                                if (item.contieneError == false)
                                {
                                    eDocument = item.eDocument;
                                    respuestaPositiva = true;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(eDocument))
                        {
                            await PDFsS3(referencia, itemFact.CONS_FACT, itemFact.NUM_REM, pMisDocumentos, GObjUsuario);
                        }

                        listRespuestas.Add(respuestaPositiva);

                    }

                }

                if (listRespuestas.Count == 0)
                {
                    return false;
                }
                else if (listRespuestas.Contains(false))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error al recibir cove: " + ex.Message);
            }

        }


        public async Task PDFsS3(Referencias objRefe, int ConsFact, int NumRem, string pMisDocumentos, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {            
            if (string.IsNullOrEmpty(pMisDocumentos))
            {
                var objUbicacion = new UbicaciondeArchivos();
                var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                objUbicacion = objUbicacionD.Buscar(121);
                if (!(objUbicacion == null))
                {
                    pMisDocumentos = objUbicacion.Ubicacion + @"\" + GObjUsuario.Usuario.Trim() + @"\ExperttiTmp";
                }
            }
            string Directorio = pMisDocumentos;
            var objRepAsync = new ReportesAsync(_configuration);

            try
            {
                if (Directory.Exists(Directorio) == false)
                {
                    Directory.CreateDirectory(Directorio);
                }

                int Result = 0;
                var objCentralizar = new CentralizarDocsS3(_configuration);


                var objCove = new FacturasCove();
                var objCoveD = new FacturasCoveRepository(_configuration);
                objCove = objCoveD.Buscar(objRefe.IDReferencia, ConsFact);
                if (objCove == null)
                {
                    throw new ArgumentException("No existe Cove para la factura solicitada!");
                }

                var objfactur = new SaaioFactur();
                var objfacturD = new SaaioFacturExperttiRepository(_configuration);
                objfactur = objfacturD.Buscar(objRefe.NumeroDeReferencia, ConsFact);
                if (objfactur == null)
                {
                    throw new ArgumentException("hubo un error para localizar la factura");
                }

                string ArchivoCove = string.Empty;
                string ArchivoAcuse = string.Empty;

                if (objfactur.REL_FACT == "S")
                {
                    // Es una Remesa
                    ArchivoCove = await objRepAsync.GenerarAcusesCoveRemesas(34, objRefe.IDReferencia, ConsFact, GObjUsuario.Usuario.Trim());
                    ArchivoAcuse = await objRepAsync.GenerarAcusesCoveRemesas(35, objRefe.IDReferencia, ConsFact, GObjUsuario.Usuario.Trim());

                    Result = await objCentralizar.AgregarDocumentos(ArchivoCove, objRefe, 1150, "", objCove.IDFacturaCOVE, GObjUsuario, false, "");

                    var objSaaioFacturD = new SaaioFacturRepository(_configuration);
                    var lstSaiioFact = new List<SaaioFactur>();
                    lstSaiioFact = objSaaioFacturD.CargarRemesa(objRefe.NumeroDeReferencia.Trim(), NumRem);
                    foreach (SaaioFactur ItemFac in lstSaiioFact)
                        objCoveD.ModificarDocCOVE(objRefe.IDReferencia, ItemFac.CONS_FACT, Result);

                    Result = await objCentralizar.AgregarDocumentos(ArchivoAcuse, objRefe, 1149, "", objCove.IDFacturaCOVE, GObjUsuario, false, "");
                    foreach (SaaioFactur ItemFac in lstSaiioFact)
                        objCoveD.ModificarDocAcuse(objRefe.IDReferencia, ItemFac.CONS_FACT, Result);
                }

                else
                {
                    // Es un COVE "normal"
                    ArchivoCove = await objRepAsync.GenerarAcusesCove(30, objRefe.IDReferencia, ConsFact.ToString(), GObjUsuario.Usuario);
                    ArchivoAcuse = await objRepAsync.GenerarAcusesCove(31, objRefe.IDReferencia, ConsFact.ToString(), GObjUsuario.Usuario);

                    Result = await objCentralizar.AgregarDocumentos(ArchivoCove, objRefe, 1150, "", objCove.IDFacturaCOVE, GObjUsuario, false, "");
                    objCoveD.ModificarDocCOVE(objRefe.IDReferencia, ConsFact, Result);

                    Result = await objCentralizar.AgregarDocumentos(ArchivoAcuse, objRefe, 1149, "", objCove.IDFacturaCOVE, GObjUsuario, false, "");
                    objCoveD.ModificarDocAcuse(objRefe.IDReferencia, ConsFact, Result);
                }
            }

            catch (Exception ex)
            {
                throw new ArgumentException("No genero PDF " + ex.Message);
            }
        }
        public List<int> BuscarGuiasMaster(string Referencia)
        {
            var lstTiposdePedimentos = new List<int>();
            SqlDataReader dataReader;

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_NUBE_PREPAGO_SEARCH_MASTER";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@Referencia", SqlDbType.VarChar, 25);
                    @param.Value = Referencia;

                    dap.Fill(dtb);

                    if (dtb.Rows.Count > 0)
                    {
                        foreach (DataRow fila in dtb.Rows)
                            lstTiposdePedimentos.Add(Convert.ToInt32(fila["IDMasterConsol"]));
                    }
                    else
                    {
                        lstTiposdePedimentos = null;
                    }



                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();

                    cn.Dispose();
                    throw new Exception(ex.Message.ToString());
                }

            }
            return lstTiposdePedimentos;
        }

        public Fecha FechaDelServidor()
        {
            var objFecha = new Fecha();
            var objHelper = new Helper();

            try
            {
                using (SqlConnection cn = new SqlConnection(SConexion))
                using (SqlCommand cmd = new SqlCommand("fecha_y_hora_del_servidor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            objFecha.mes = objHelper.PADL(dr["mes"].ToString(), 2, "0", false);
                            objFecha.dia = objHelper.PADL(dr["dia"].ToString(), 2, "0", false);
                            objFecha.anio = dr["ano"].ToString();
                            objFecha.hora = objHelper.PADL(dr["hora"].ToString(), 2, "0", false);
                            objFecha.minuto = objHelper.PADL(dr["minuto"].ToString(), 2, "0", false);
                            objFecha.segundo = objHelper.PADL(dr["segundo"].ToString(), 2, "0", false);
                            objFecha.milisegundo = objHelper.PADL(dr["milisegundo"].ToString(), 3, "0", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar leer: " + ex.Message);
            }

            return objFecha;
        }



        public async Task<bool> sellarGuia(int IdReferencia, int idCheckpoint, string pMisDocumentos, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {
            var objPaths = new PathsDocumentos();
            var objDocD = new DocumentosRepository(_configuration);
            var objRefe = new Referencias();
            var objRefeD = new ReferenciasRepository(_configuration);
            string RutaArchivoTIF = string.Empty;
            string Identificador = "C2";
            try
            {
                objRefe = objRefeD.Buscar(IdReferencia, GObjUsuario.IDDatosDeEmpresa);

                if (idCheckpoint == 20 & objRefe.IdMasterConsol == 0)
                {
                    throw new ArgumentException("Esta guia no puede ser turnada hasta que se Asigne una guia Master ");
                }



                string Archivo = objRefe.NumeroDeReferencia + "_" + "GH_01" + ".pdf";
                objPaths = objDocD.BuscarPath(objRefe.NumeroDeReferencia, "C2");

                string ArchivoGuia = string.Empty;
                ArchivoGuia = objPaths.pathArchivo + objRefe.NumeroDeReferencia + "_C2_01.TIF";

                var ObjPdf = new CentralizarDocs(_configuration);
                if (ObjPdf.ExisteDocumento(ArchivoGuia) == false)
                {
                    string ArchivoGuiaAnterior = string.Empty;
                    ArchivoGuiaAnterior = objPaths.PathArchivoAnterior + objRefe.NumeroDeReferencia + "_C2_01.TIF";
                    if (File.Exists(ArchivoGuiaAnterior))
                    {
                        File.Move(ArchivoGuiaAnterior, ArchivoGuia);
                    }
                    else
                    {
                        throw new ArgumentException("No existe una guia para sellar " + ArchivoGuia);

                    }
                }

                if (ObjPdf.ExisteDocumento(objPaths.pathArchivo + Archivo)) // ya existe una guia sellada
                {
                    throw new ArgumentException("Ya existe una guía sellada");
                }


                string ArchivoNuevo = string.Empty;

                var listadoMaster = BuscarGuiasMaster(objRefe.NumeroDeReferencia);
                if (listadoMaster == null)
                {
                    throw new ArgumentException("No tiene una master asociada");
                }
                else
                {
                    foreach (int itemMasterId in listadoMaster)
                    {
                        ArchivoNuevo = ObjPdf.SellarGuia(itemMasterId, ArchivoGuia);

                        string Destino = string.Empty;
                        if (GObjUsuario.Oficina.ironPDF)
                        {
                            var objConvertira300 = new ClConvertir300dpi();
                            Destino = objConvertira300.ConvertirPDF(ArchivoNuevo, pMisDocumentos + @"\" + DateTime.Now.ToString("yyyyMMdd") + @"\", objRefe.NumeroDeReferencia + "_300_" + itemMasterId.ToString() + ".pdf");
                        }

                        else
                        {
                            byte[] ArchivoSellado;
                            ArchivoSellado = ObjPdf.DescargarDocumento(ArchivoNuevo);

                            var wsCentralizar = new wsCentralizar.IwsDocumentosClient();
                            Destino = await wsCentralizar.SubirArchivoAsync(Archivo, ArchivoSellado, ".pdf", false, SConexion);
                        }

                        int Result;
                        var objDocumentosporguia = new wsCentralizar.DocumentosPorGuia();
                        var objCentralizar = new CentralizarS3sp(_configuration);
                        Result = await objCentralizar.AgregarDocumentos(Destino, IdReferencia, 1047, "", 0, GObjUsuario, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("sellarGuia: " + ex.Message);
            }
            return true;
        }

        public async Task<string> Convertira300dpis(string ArchivoPDFaConvertir, string NombreArchivo, string RutaVucem, LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {
            string NuevoPdf = string.Empty;

            var objUbicacionDpis = new UbicaciondeArchivos();
            var objUbicacionDpisD = new UbicaciondeArchivosRepository(_configuration);
            objUbicacionDpis = objUbicacionDpisD.Buscar(37);


            if (Directory.Exists(objUbicacionDpis.Ubicacion.Trim()) == false)
            {
                throw new ArgumentException("No fue posible alcanzar la ruta de acceso :" + objUbicacionDpis.Ubicacion.Trim());
            }

            if (GObjUsuario.Oficina.ironPDF)
            {
                var objConvertira300 = new ClConvertir300dpi();

                string Archivo300dpis = string.Empty;
                Archivo300dpis = objConvertira300.ConvertirPDF(ArchivoPDFaConvertir, objUbicacionDpis.Ubicacion.Trim(), NombreArchivo);

                NuevoPdf = Archivo300dpis;
            }
            else
            {
                byte[] Arch = File.ReadAllBytes(ArchivoPDFaConvertir);
                var wsCentralizar = new IwsDocumentosClient();

                NuevoPdf = await wsCentralizar.ConvertirPDFdpiNuevoAsync(Path.GetDirectoryName(ArchivoPDFaConvertir), Path.GetFileNameWithoutExtension(ArchivoPDFaConvertir), Arch, Path.GetExtension(ArchivoPDFaConvertir), GObjUsuario.IdUsuario, RutaVucem, SConexion);
            }

            return NuevoPdf;
        }

        



    }
}
