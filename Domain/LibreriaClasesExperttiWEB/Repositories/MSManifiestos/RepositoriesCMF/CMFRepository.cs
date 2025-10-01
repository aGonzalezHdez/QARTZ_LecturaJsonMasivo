//using Chilkat;
using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesSifty;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{
    public class CMFRepository : ICMFRepository
    {
        public string sConexion { get; set; }
        string ICMFRepository.sConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;


        int totalGuias = 0;
        int GuiasGuardadas = 0;
        public CMFRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public List<string> Subir(string ArchivoCMF, string NombreArchivo)
        {
            List<string> lstErrores = new();


            try
            {
                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(247);


                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }

                string Archivo;
                Archivo = objRuta.Ubicacion + NombreArchivo;

                byte[] bytes = Convert.FromBase64String(ArchivoCMF);
                System.IO.File.WriteAllBytes(Archivo, bytes);
                FileInfo newFile = new FileInfo(Archivo);


                if (newFile.Exists == false)
                {
                    lstErrores.Add("No se encontró el Archivo");
                }

                else
                {
                    string jsonString = System.IO.File.ReadAllText(Archivo.Trim());
                    cmfEncabezado? objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<cmfEncabezado>(jsonString);
                    if (objRespuesta != null)
                    {
                        lstErrores = Guardar(objRespuesta);
                    }


                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstErrores;
        }

        public List<string> Guardar(cmfEncabezado objNuevo)
        {
            List<string> lstErrores = new();

            try
            {
                if (objNuevo == null)
                {
                    lstErrores.Add("Archivo:" + " Error : " + "No deserializo");
                    throw new Exception("objeto nulo");
                }

              
                CustomerMasterFileRepository customerMasterFileD = new CustomerMasterFileRepository(_configuration);

                if (objNuevo.shipments == null)
                {
                    lstErrores.Add("Archivo:" + objNuevo.File.Trim() + " Error : " + "No deserializo");
                }
                totalGuias += objNuevo.shipments.Count;

                foreach (var shipment in objNuevo.shipments)
                {
                    CustomerMasterFile customerMasterFile = new CustomerMasterFile();
                    string guiaHouse = shipment.hAWB.Trim();
                    try
                    {

                        customerMasterFile.GuiaHouse = shipment.hAWB.Trim();
                        customerMasterFile.GtwDestino = objNuevo.gateway.Trim();
                        customerMasterFile.IataOrigen = shipment.shpOrgn.Trim();
                        customerMasterFile.IataDestino = shipment.shpDest.Trim();
                        customerMasterFile.TipoEnvio = shipment.prodContentCd.Trim();
                        if (shipment.cargodesc == null)
                        { customerMasterFile.Descripcion = ""; }
                        else { customerMasterFile.Descripcion = shipment.cargodesc.Trim(); }

                        if (shipment.consignor.cnorDHLAcctNo == null)
                        {
                            customerMasterFile.NoCuenta = "";
                        }
                        else { customerMasterFile.NoCuenta = shipment.consignor.cnorDHLAcctNo.Trim(); }

                        customerMasterFile.Destinatario = shipment.consignee.cneeCoName.Trim();
                        customerMasterFile.Direccion1 = shipment.consignee.cneeAddrLn1.Trim();


                        if (shipment.consignee.cneeAddrLn2 == null)
                        { customerMasterFile.Direccion2 = ""; }
                        else { customerMasterFile.Direccion2 = shipment.consignee.cneeAddrLn2.Trim(); }

                        if (shipment.consignee.cneeAddrLn3 == null)
                        { customerMasterFile.Direccion3 = ""; }
                        else { customerMasterFile.Direccion3 = shipment.consignee.cneeAddrLn3.Trim(); }

                        customerMasterFile.Ciudad = shipment.consignee.cneeCtryCd.Trim();


                        if (shipment.consignee.cneePostalCd == null)
                        { customerMasterFile.CodigoPostal = ""; }
                        else { customerMasterFile.CodigoPostal = shipment.consignee.cneePostalCd.Trim(); }


                        customerMasterFile.Pais = shipment.consignee.cneeCtryCd.Trim();
                        if (shipment.consignee.cneePreferredName == null) { customerMasterFile.Contacto = ""; }
                        else { customerMasterFile.Contacto = shipment.consignee.cneePreferredName.Trim(); }

                        string MedioContacto = "Tel";

                        //if (shipment.consignee.cneeDeviceTyp == null) { MedioContacto = ""; }
                        //else { MedioContacto = shipment.consignee.cneeDeviceTyp.Trim(); }

                        string DatosContacto = string.Empty;

                        if (shipment.consignee.cneeTel == null) { DatosContacto = ""; }
                        else { DatosContacto = shipment.consignee.cneeTel.Trim(); }

                        customerMasterFile.MedioContacto = MedioContacto;
                        customerMasterFile.DatosContacto = DatosContacto;
                        customerMasterFile.Proveedor = shipment.consignor.cnorCoName.Trim();
                        customerMasterFile.ProveedorDireccion = shipment.consignor.cnorAddrLn1.Trim();

                        if (shipment.consignor.cnorAddrLn2 == null) { customerMasterFile.ProveedorInterior = ""; }
                        else { customerMasterFile.ProveedorInterior = shipment.consignor.cnorAddrLn2.Trim(); }

                        customerMasterFile.ProveedorCiudad = shipment.consignor.cnorAddrLn3.Trim();
                        customerMasterFile.ProveedorEstado = shipment.consignor.cnorCity.Trim();
                        customerMasterFile.ProveedorPais = shipment.consignor.cnorCtryCd.Trim();
                        customerMasterFile.ProveedorCodigoPostal = "";
                        customerMasterFile.ProveedorMedio = shipment.consignor.cnorDeviceTyp == null ? "" : shipment.consignor.cnorDeviceTyp.Trim(); //shipment.consignor.cnorDeviceTyp;
                        customerMasterFile.ProveedorDatos = shipment.consignor.cnorDeviceDtls == null ? "" : shipment.consignor.cnorDeviceDtls.Trim();
                        customerMasterFile.Peso = Convert.ToDouble(shipment.actWgt);
                        customerMasterFile.PesoVolumetrico = Convert.ToDouble(shipment.actWgt);
                        customerMasterFile.Piezas = Convert.ToInt32(shipment.sDPieces);

                        if (shipment.incoterms == null) { customerMasterFile.Incoterm = ""; }
                        else { customerMasterFile.Incoterm = shipment.incoterms.Trim(); }

                        customerMasterFile.ServicioDhl = shipment.dHLServiceCd.Trim();
                        customerMasterFile.FacturaValor = Convert.ToDouble(shipment.cstmsVal);
                        customerMasterFile.FacturaMoneda = shipment.cstmsValCrncyCd.Trim();
                        customerMasterFile.PaisVendedor = "";
                        customerMasterFile.PaisComprador = "";
                        customerMasterFile.NombredeArchivo = objNuevo.File.Trim();
                        customerMasterFile.ShipmentReference = "";

                        customerMasterFile.NoCuentaCliente = shipment.payer.payerDHLAcctNo == null? "": shipment.payer.payerDHLAcctNo.Trim();
                        customerMasterFile.Frght = 0;
                        customerMasterFile.FrghtCrncy = "";
                        customerMasterFile.Partidas = shipment.lineItems.Count;
                        customerMasterFile.TaxIDImpo = shipment.consignee.cneeVatNo == null ? "" : shipment.consignee.cneeVatNo.Trim();

                        int idCMF = customerMasterFileD.Insertar(customerMasterFile);
                        customerMasterFileD.modificarRFC(shipment.hAWB.Trim(), shipment.consignee.cneeVatNo, shipment.consignee.cneeTel, shipment.consignee.cneeEmail);

                        ApiSiftyRepository objSifty = new(_configuration);
                        bool respuesta = objSifty.EnviarSifty(idCMF);

                        foreach (cmfLineItems PartidasCMF in shipment.lineItems)
                        {
                            try
                            {
                              CMFPartidasRepository objPartidas = new(_configuration);
                              int idPartidasCMF = objPartidas.Insertar(PartidasCMF, idCMF);
                            

                                if (PartidasCMF.descOfGoods != null)
                                {
                                    customerMasterFileD.SinonimosdeRiesgo(customerMasterFile.GuiaHouse, PartidasCMF.descOfGoods.Trim(), idPartidasCMF);

                                    if (PartidasCMF.ctryMfctrerOrgn != null)
                                    {
                                        if (PartidasCMF.ctryMfctrerOrgn.Trim() == "CN")
                                        {
                                            customerMasterFileD.modificarPorOrigen(idCMF, PartidasCMF.ctryMfctrerOrgn.Trim(), PartidasCMF.descOfGoods.Trim());
                                        }
                                    }
                                }     
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                        foreach (cmfPieces itempieces in shipment.pieces)
                        {
                            if (itempieces != null)
                            {
                                customerMasterFileD.InsertarPieceIdCMF(itempieces);
                            }
                        }


                        GuiasGuardadas += 1;
                    }
                    catch (Exception ex)
                    {
                        lstErrores.Add("Archivo:" + objNuevo.File.Trim() + " Guia House : " + guiaHouse.Trim() + " Error : " + ex.Message);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstErrores;



        }


        public ManifiestosErrores SubirSDAAAporFTP()
        {
            ManifiestosErrores obj = new ManifiestosErrores();
            List<string> lstErrores = new();
            List<string> lstErrorestodos = new();
            try
            {
                BitacoraJsonCMF objBitacoraJsonCMF = new BitacoraJsonCMF();
                BitacoraJsonCMFRepository objBitacoraJsonCMFD = new BitacoraJsonCMFRepository(_configuration);
                objBitacoraJsonCMF = objBitacoraJsonCMFD.BuscarActivos();

                if (objBitacoraJsonCMF != null)
                {
                    throw new Exception("Se encuentra un proceso activo el cúal inicio " + objBitacoraJsonCMF.IniciaProceso.ToString("dd/MM/yyyy HH:mm:ss"));

                }
                else
                {
                    objBitacoraJsonCMFD.Insertar();
                }

                try
                {
                    DescargaFTPRepository objDescarga = new(_configuration);
                    objDescarga.descargarFTP(40);
                }
                catch (Exception err)
                {
                    lstErrorestodos.Add(err.Message.ToString());
                }

                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(247);


                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    throw new Exception("El directorio no esta disponible");
                }

                string DirectorioNuevo = objRuta.Ubicacion.Trim() + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                if (Directory.Exists(DirectorioNuevo) == false)
                {
                    Directory.CreateDirectory(DirectorioNuevo);
                }

                int ArchivosProcesados = 0;
                foreach (string Archivo in Directory.GetFiles(objRuta.Ubicacion.Trim()))
                {
                    try
                    {
                        if (Archivo.Length > 0)
                        {
                            if (Path.GetExtension(Archivo) == ".json")
                            {
                                string jsonString = System.IO.File.ReadAllText(Archivo.Trim());
                                cmfEncabezado objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<cmfEncabezado>(jsonString);
                                lstErrores = Guardar(objRespuesta);

                                foreach (string str in lstErrores)
                                {
                                    lstErrorestodos.Add(str);
                                }


                                ArchivosProcesados += 1;
                                if (System.IO.File.Exists(Archivo))
                                {
                                    System.IO.File.Move(Archivo, DirectorioNuevo + "/" + Path.GetFileName(Archivo));
                                }
                            }

                        }

                    }
                    catch (Exception err)
                    {

                        string directorioerror = DirectorioNuevo + "/error/";
                        if (Directory.Exists(directorioerror) == false)
                        { Directory.CreateDirectory(directorioerror); }

                        System.IO.File.Move(Archivo, directorioerror + Path.GetFileName(Archivo));
                        lstErrores.Add(err.Message.Trim());
                    }


                }
                obj.txtTotalError = lstErrorestodos.Count();
                obj.lstErrores = lstErrorestodos;
                obj.txtFilas = totalGuias;
                obj.txtProcesadas = GuiasGuardadas;

                BitacoraJsonCMF objBitacoraJsonCMFfin = new BitacoraJsonCMF();
                objBitacoraJsonCMFfin.Guias = totalGuias;
                objBitacoraJsonCMFfin.Archivos = ArchivosProcesados;
                objBitacoraJsonCMFD.Modificar(objBitacoraJsonCMFfin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }



            return obj;
        }

        public ManifiestosErrores SubirSDAAAporDirectorio()
        {
            ManifiestosErrores obj = new ManifiestosErrores();
            List<string> lstErrores = new();
            List<string> lstErrorestodos = new();
            try
            {
             
               
                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(247);


                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    throw new Exception("El directorio no esta disponible");
                }

                string DirectorioNuevo = objRuta.Ubicacion.Trim() + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                if (Directory.Exists(DirectorioNuevo) == false)
                {
                    Directory.CreateDirectory(DirectorioNuevo);
                }

                int ArchivosProcesados = 0;
                foreach (string Archivo in Directory.GetFiles(objRuta.Ubicacion.Trim()))
                {
                    try
                    {
                        if (Archivo.Length > 0)
                        {
                            if (Path.GetExtension(Archivo) == ".json")
                            {
                                string jsonString = System.IO.File.ReadAllText(Archivo.Trim());
                                cmfEncabezado objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<cmfEncabezado>(jsonString);
                                lstErrores = Guardar(objRespuesta);

                                foreach (string str in lstErrores)
                                {
                                    lstErrorestodos.Add(str);
                                }


                                ArchivosProcesados += 1;
                                if (System.IO.File.Exists(Archivo))
                                {
                                    System.IO.File.Move(Archivo, DirectorioNuevo + "/" + Path.GetFileName(Archivo));
                                }
                            }

                        }

                    }
                    catch (Exception err)
                    {

                        string directorioerror = DirectorioNuevo + "/error/";
                        if (Directory.Exists(directorioerror) == false)
                        { Directory.CreateDirectory(directorioerror); }

                        System.IO.File.Move(Archivo, directorioerror + Path.GetFileName(Archivo));
                        lstErrores.Add(err.Message.Trim());
                    }


                }
                obj.txtTotalError = lstErrorestodos.Count();
                obj.lstErrores = lstErrorestodos;
                obj.txtFilas = totalGuias;
                obj.txtProcesadas = GuiasGuardadas;

               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }



            return obj;
        }

 

        public DataTable CargarDatosContactoCliente(int IdDtosEmpresa, string Guia)
        {
            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_SEARCH_DATOSCONTACTOCLIENTECMF";


                    @param = dap.SelectCommand.Parameters.Add("@IDE", SqlDbType.Int, 4);
                    @param.Value = IdDtosEmpresa;

                    @param = dap.SelectCommand.Parameters.Add("@GUIA_HOUSE", SqlDbType.VarChar, 15);
                    @param.Value = Guia;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_SEARCH_DATOSCONTACTOCLIENTECMF");
                }

            }

            return dtb;
        }

        public bool ModificarDatosContactoCliente(int IdCMF, string RFC, string Telefono, string Email)
        {
            bool bUpdate = false;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_RFC_PORID";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@IdCMF", SqlDbType.Int, 4);
            param.Value = IdCMF;

            param = cmd.Parameters.Add("@DatosContacto", SqlDbType.VarChar, 30);
            param.Value = Telefono == null ? "" : Telefono;

            param = cmd.Parameters.Add("@TaxIDImpo", SqlDbType.VarChar, 50);
            param.Value = RFC == null ? "" : RFC;

            param = cmd.Parameters.Add("@destinatarioEmail", SqlDbType.VarChar, 100);
            param.Value = Email == null ? "" : Email;

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    bUpdate = true;
                else
                    bUpdate = false;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                bUpdate = false;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CUSTOMERMASTERFILE_RFC_PORID");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return bUpdate;
        }

    }
}
