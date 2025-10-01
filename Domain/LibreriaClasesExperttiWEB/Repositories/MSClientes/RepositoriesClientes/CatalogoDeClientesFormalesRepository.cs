using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeClientesFormalesRepository : ICatalogoDeClientesFormalesRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeClientesFormalesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeClientesFormalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeClientesFormales GetClientesPorClave(string Clave)
        {
            CatalogoDeClientesFormales ClientesPorClave = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CLIENTES_POR_CLAVE_JURIDICO_TOP", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar).Value = Clave;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();

                        ClientesPorClave.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        ClientesPorClave.Clave = string.Format("{0}", dr["Clave"]);
                        ClientesPorClave.Nombre = string.Format("{0}", dr["Nombre"]);
                        ClientesPorClave.ApellidoPaterno = string.Format("{0}", dr["ApellidoPaterno"]);
                        ClientesPorClave.ApellidoMaterno = string.Format("{0}", dr["ApellidoMaterno"]);
                        ClientesPorClave.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                        ClientesPorClave.RFC = string.Format("{0}", dr["RFC"]);
                        ClientesPorClave.CURP = string.Format("{0}", dr["CURP"]);
                        ClientesPorClave.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                        ClientesPorClave.Activo = Convert.ToBoolean(dr["Activo"]);
                        ClientesPorClave.Telefono = string.Format("{0}", dr["Telefono"]);
                        ClientesPorClave.EmailContacto = string.Format("{0}", dr["EmailContacto"]);
                        ClientesPorClave.Atencion = string.Format("{0}", dr["Atencion"]);
                        ClientesPorClave.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                        ClientesPorClave.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                        ClientesPorClave.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                        ClientesPorClave.eMailCFD = string.Format("{0}", dr["eMailCFD"]);
                        ClientesPorClave.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                        ClientesPorClave.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                        ClientesPorClave.SectorComercial = string.Format("{0}", dr["SectorComercial"]);
                        ClientesPorClave.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                        ClientesPorClave.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                        ClientesPorClave.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                        ClientesPorClave.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                        ClientesPorClave.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                        ClientesPorClave.EmailPdfCASA = string.Format("{0}", dr["EmailPdfCASA"]);
                        ClientesPorClave.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        ClientesPorClave.RfcParaConsulta = string.Format("{0}", dr["RfcParaConsulta"]);
                        ClientesPorClave.EmailManifiesto = string.Format("{0}", dr["EmailManifiesto"]);
                        ClientesPorClave.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                        ClientesPorClave.SoloDeGrupoEI = Convert.ToInt32(dr["SoloDeGrupoEI"]);
                        ClientesPorClave.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                        ClientesPorClave.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                        ClientesPorClave.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                        ClientesPorClave.Prospecto = Convert.ToBoolean(dr["Prospecto"]);
                        ClientesPorClave.IdUsuarioManif = Convert.ToInt32(dr["IdUsuarioManif"]);
                        ClientesPorClave.IDTipoCliente = Convert.ToInt32(dr["IDTipoCliente"]);
                        ClientesPorClave.IDTipoClienteTop = Convert.ToInt32(dr["IDTipoClienteTop"]);
                        ClientesPorClave.IdRegimenFiscal = Convert.ToInt32(dr["IdRegimenFiscal"]);
                        ClientesPorClave.RegimenCapital = string.Format("{0}", dr["RegimenCapital"]);
                        ClientesPorClave.SoloFacturacion = Convert.ToBoolean(dr["SoloFacturacion"]);
                        ClientesPorClave.idCIF = string.Format("{0}", dr["idCIF"]);
                        ClientesPorClave.Ultimo = string.Format("{0}", dr["Ultimo"]);
                        ClientesPorClave.Primero = string.Format("{0}", dr["Primero"]);
                        ClientesPorClave.Siguiente = string.Format("{0}", dr["Siguiente"]);
                        ClientesPorClave.Anterior = string.Format("{0}", dr["Anterior"]);

                        int idCliente = ClientesPorClave.IDCliente;
                        DireccionesDeClientesRepository DireccionesDeClientesRepository = new(_configuration);
                        ClientesPorClave.DireccionesDeClientes = DireccionesDeClientesRepository.BuscarDireccionActiva(idCliente);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ClientesPorClave;
        }

        public CatalogoDeClientesFormales Buscar(int MyIdCliente)
        {
            CatalogoDeClientesFormales objCatalogoDeClientesFormales = new();

            try
            {

                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGODECLIENTESEXPERTTIXID", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = MyIdCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCatalogoDeClientesFormales.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objCatalogoDeClientesFormales.Clave = string.Format("{0}", dr["Clave"]);
                        objCatalogoDeClientesFormales.Nombre = string.Format("{0}", dr["Nombre"]);
                        objCatalogoDeClientesFormales.ApellidoPaterno = string.Format("{0}", dr["ApellidoPaterno"]);
                        objCatalogoDeClientesFormales.ApellidoMaterno = string.Format("{0}", dr["ApellidoMaterno"]);
                        objCatalogoDeClientesFormales.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                        objCatalogoDeClientesFormales.RFC = string.Format("{0}", dr["RFC"]);
                        objCatalogoDeClientesFormales.CURP = string.Format("{0}", dr["CURP"]);
                        objCatalogoDeClientesFormales.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                        objCatalogoDeClientesFormales.Activo = Convert.ToBoolean(dr["Activo"]);
                        objCatalogoDeClientesFormales.Telefono = string.Format("{0}", dr["Telefono"]);
                        objCatalogoDeClientesFormales.EmailContacto = string.Format("{0}", dr["EmailContacto"]);
                        objCatalogoDeClientesFormales.Atencion = string.Format("{0}", dr["Atencion"]);
                        objCatalogoDeClientesFormales.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                        objCatalogoDeClientesFormales.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                        objCatalogoDeClientesFormales.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                        objCatalogoDeClientesFormales.eMailCFD = string.Format("{0}", dr["eMailCFD"]);
                        objCatalogoDeClientesFormales.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                        objCatalogoDeClientesFormales.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                        objCatalogoDeClientesFormales.SectorComercial = string.Format("{0}", dr["SectorComercial"]);
                        objCatalogoDeClientesFormales.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                        objCatalogoDeClientesFormales.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                        objCatalogoDeClientesFormales.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                        objCatalogoDeClientesFormales.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                        objCatalogoDeClientesFormales.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                        objCatalogoDeClientesFormales.EmailPdfCASA = string.Format("{0}", dr["EmailPdfCASA"]);
                        objCatalogoDeClientesFormales.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        objCatalogoDeClientesFormales.RfcParaConsulta = string.Format("{0}", dr["RfcParaConsulta"]);
                        objCatalogoDeClientesFormales.EmailManifiesto = string.Format("{0}", dr["EmailManifiesto"]);
                        objCatalogoDeClientesFormales.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                        objCatalogoDeClientesFormales.SoloDeGrupoEI = Convert.ToInt32(dr["SoloDeGrupoEI"]);
                        objCatalogoDeClientesFormales.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                        objCatalogoDeClientesFormales.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                        objCatalogoDeClientesFormales.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                        objCatalogoDeClientesFormales.Prospecto = Convert.ToBoolean(dr["Prospecto"]);
                        objCatalogoDeClientesFormales.IdUsuarioManif = Convert.ToInt32(dr["IdUsuarioManif"]);
                        objCatalogoDeClientesFormales.IdRegimenFiscal = Convert.ToInt32(dr["IdRegimenFiscal"]);
                        objCatalogoDeClientesFormales.RegimenCapital = string.Format("{0}", dr["RegimenCapital"]);                       
                    }
                    else
                    {
                        objCatalogoDeClientesFormales = null;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCatalogoDeClientesFormales;
        }

        public bool GetExisteClientePorRFC(string RFC)
        {
            int Respuesta;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SABER_SI_YA_EXISTE_RFC_EN_CATALOGODECLIENTESEXPERTTI", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 13).Value = RFC;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        Respuesta = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        cmd.Parameters.Clear();
                        if (Respuesta == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string Insertar(CatalogoDeClientesFormales objCatalogoDeClientes)
        {
            string MyClaveCliente = "";
            Errores Errores = new();

            try
            {
                ValidaObjeto(objCatalogoDeClientes, true);

                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CATALOGODECLIENTESEXPERTTI_TOP", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 120).Value = objCatalogoDeClientes.Nombre;
                    cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.ApellidoPaterno.Trim();
                    cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.ApellidoMaterno.Trim();
                    cmd.Parameters.Add("@RFCGenerico", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.RFCGenerico;
                    cmd.Parameters.Add("@RFC", SqlDbType.Char, 13).Value = objCatalogoDeClientes.RFC.Trim();
                    cmd.Parameters.Add("@CURP", SqlDbType.Char, 18).Value = objCatalogoDeClientes.CURP.Trim();
                    cmd.Parameters.Add("@IDCapturo", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDCapturo;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.Activo;
                    cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.Telefono.Trim();
                    cmd.Parameters.Add("@EmailContacto", SqlDbType.VarChar, 250).Value = objCatalogoDeClientes.EmailContacto.Trim();
                    cmd.Parameters.Add("@Atencion", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.Atencion.Trim();
                    cmd.Parameters.Add("@VerificarProductos", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.VerificarProductos;
                    cmd.Parameters.Add("@IDNotificador", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDNotificador;
                    cmd.Parameters.Add("@IDNotificadorBK", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDNotificadorBK;
                    cmd.Parameters.Add("@VerificaProveedor", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.VerificaProveedor;
                    cmd.Parameters.Add("@OperacionesRT", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.OperacionesRT;
                    cmd.Parameters.Add("@SectorComercial", SqlDbType.VarChar, 8000).Value = objCatalogoDeClientes.SectorComercial.Trim();
                    cmd.Parameters.Add("@MandanEdocuments", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.MandanEdocuments;
                    cmd.Parameters.Add("@CambianFacturas", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.CambianFacturas;
                    cmd.Parameters.Add("@ForzarSOP", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.ForzarSOP;
                    cmd.Parameters.Add("@EnPadronDeIMP", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.EnPadronDeIMP;
                    cmd.Parameters.Add("@NoFacturar", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.NoFacturar;
                    cmd.Parameters.Add("@SoloExportacion", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.SoloExportacion;
                    cmd.Parameters.Add("@Prospecto", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.Prospecto;
                    cmd.Parameters.Add("@IdUsuarioManif", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IdUsuarioManif;
                    cmd.Parameters.Add("@IDTipoCliente", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDTipoCliente;
                    cmd.Parameters.Add("@IDTipoClienteTop", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDTipoClienteTop;
                    cmd.Parameters.Add("@IdRegimenFiscal", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IdRegimenFiscal;
                    cmd.Parameters.Add("@RegimenCapital", SqlDbType.VarChar, 100).Value = objCatalogoDeClientes.RegimenCapital.Trim();
                    cmd.Parameters.Add("@SoloFacturacion", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.SoloFacturacion;
                    cmd.Parameters.Add("@idCIF", SqlDbType.VarChar, 30).Value = objCatalogoDeClientes.idCIF.Trim();
                    cmd.Parameters.Add("@newid_registro", SqlDbType.VarChar, 6).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {

                        if (Convert.ToString(cmd.Parameters["@newid_registro"].Value) != "")
                        {
                            MyClaveCliente = Convert.ToString(cmd.Parameters["@newid_registro"].Value)!;
                        }
                        else
                        {
                            Errores.IdError = 3;
                            Errores.Error = $"No fue posible insertar el cliente";
                            var json = JsonConvert.SerializeObject(Errores);
                            throw new Exception(json);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errores.IdError = 3;
                Errores.Error = $"No fue posible insertar el cliente por : " + ex.Message.ToString();
                var json = JsonConvert.SerializeObject(Errores);
                throw new Exception(json);
            }
            return MyClaveCliente;
        }

        public class Errores
        {
            public int IdError { get; set; }
            public string Error { get; set; }
        }

        public int Modificar(CatalogoDeClientesFormales objCatalogoDeClientes)
        {
            //string Error = string.Empty;
            int id = 0;
            ValidaObjeto(objCatalogoDeClientes, false);

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_UPDATE_CATALOGODECLIENTESEXPERTTI_TOP", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDCliente;
                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 6).Value = objCatalogoDeClientes.Clave.Trim();
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 120).Value = objCatalogoDeClientes.Nombre.Trim();
                    cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.ApellidoPaterno.Trim();
                    cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.ApellidoMaterno.Trim();
                    cmd.Parameters.Add("@RFCGenerico", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.RFCGenerico;
                    cmd.Parameters.Add("@RFC", SqlDbType.Char, 13).Value = objCatalogoDeClientes.RFC.Trim();
                    cmd.Parameters.Add("@CURP", SqlDbType.Char, 18).Value = objCatalogoDeClientes.CURP.Trim();
                    cmd.Parameters.Add("@IDCapturo", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDCapturo;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.Activo;
                    cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.Telefono.Trim();
                    cmd.Parameters.Add("@EmailContacto", SqlDbType.VarChar, 250).Value = objCatalogoDeClientes.EmailContacto.Trim();
                    cmd.Parameters.Add("@Atencion", SqlDbType.VarChar, 80).Value = objCatalogoDeClientes.Atencion.Trim();
                    cmd.Parameters.Add("@VerificarProductos", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.VerificarProductos;
                    cmd.Parameters.Add("@IDNotificador", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDNotificador;
                    cmd.Parameters.Add("@IDNotificadorBK", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDNotificadorBK;
                    cmd.Parameters.Add("@eMailCFD", SqlDbType.VarChar, 8000).Value = objCatalogoDeClientes.eMailCFD.Trim();
                    cmd.Parameters.Add("@VerificaProveedor", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.VerificaProveedor;
                    cmd.Parameters.Add("@OperacionesRT", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.OperacionesRT;
                    cmd.Parameters.Add("@SectorComercial", SqlDbType.VarChar, 8000).Value = objCatalogoDeClientes.SectorComercial.Trim();
                    cmd.Parameters.Add("@TipoDeFigura", SqlDbType.Int, 4).Value = objCatalogoDeClientes.TipoDeFigura;
                    cmd.Parameters.Add("@MandanEdocuments", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.MandanEdocuments;
                    cmd.Parameters.Add("@CambianFacturas", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.CambianFacturas;
                    cmd.Parameters.Add("@ForzarSOP", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.ForzarSOP;
                    cmd.Parameters.Add("@EnPadronDeIMP", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.EnPadronDeIMP;
                    cmd.Parameters.Add("@EmailPdfCASA", SqlDbType.VarChar, 8000).Value = objCatalogoDeClientes.EmailPdfCASA.Trim();
                    cmd.Parameters.Add("@FechaDeAlta", SqlDbType.DateTime, 4).Value = objCatalogoDeClientes.FechaDeAlta;
                    cmd.Parameters.Add("@RfcParaConsulta", SqlDbType.VarChar, 8000).Value = objCatalogoDeClientes.RfcParaConsulta.Trim();
                    cmd.Parameters.Add("@EmailManifiesto", SqlDbType.VarChar, 8000).Value = objCatalogoDeClientes.EmailManifiesto.Trim();
                    cmd.Parameters.Add("@IDVendedor", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDVendedor;
                    cmd.Parameters.Add("@SoloDeGrupoEI", SqlDbType.Int, 4).Value = objCatalogoDeClientes.SoloDeGrupoEI;
                    cmd.Parameters.Add("@FechaDeInicio", SqlDbType.DateTime, 4).Value = objCatalogoDeClientes.FechaDeInicio;
                    cmd.Parameters.Add("@NoFacturar", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.NoFacturar;
                    cmd.Parameters.Add("@SoloExportacion", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.SoloExportacion;
                    cmd.Parameters.Add("@Prospecto", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.Prospecto;
                    cmd.Parameters.Add("@IdUsuarioManif", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IdUsuarioManif;
                    cmd.Parameters.Add("@IDTipoCliente", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDTipoCliente;
                    cmd.Parameters.Add("@IDTipoClienteTop", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IDTipoClienteTop;
                    cmd.Parameters.Add("@IdRegimenFiscal", SqlDbType.Int, 4).Value = objCatalogoDeClientes.IdRegimenFiscal;
                    cmd.Parameters.Add("@RegimenCapital", SqlDbType.VarChar, 8100000).Value = objCatalogoDeClientes.RegimenCapital.Trim();
                    cmd.Parameters.Add("@SoloFacturacion", SqlDbType.Bit, 4).Value = objCatalogoDeClientes.SoloFacturacion;
                    cmd.Parameters.Add("@idCIF", SqlDbType.VarChar, 30).Value = objCatalogoDeClientes.idCIF.Trim();
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        //if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        //{

                        //    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        //    {
                        //        Error = "Ok";
                        //    }
                        //    else
                        //    {                               
                        //        Error = "No se logro modificar el cliente";
                        //    }
                        //}
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public int ValidaRFC(string Rfc)
        {
            int count = 0;
            //string RFC = string.Empty;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_CATALOGODECLIENTESEXPERTTI_VALIDARFC", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = Rfc;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            //RFC = dr["RFC"].ToString();
                            count = dr.FieldCount;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
            return count;
        }

        public int ValidaCURP(string Curp)
        {
            int count = 0;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_CATALOGODECLIENTESEXPERTTI_VALIDACURP", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CURP", SqlDbType.VarChar).Value = Curp;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        count = Convert.ToInt32(dr["Count"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
            return count;
        }

        public void ValidaObjeto(CatalogoDeClientesFormales objCatalogoDeClientesFormales, bool esInsertar)
        {

            //string Error = string.Empty;

            if (objCatalogoDeClientesFormales.Clave == null)
            {
                _ = objCatalogoDeClientesFormales.Clave == "";
            }

            if (objCatalogoDeClientesFormales.Nombre == null)
            {
                _ = objCatalogoDeClientesFormales.Nombre == "";
            }

            if (objCatalogoDeClientesFormales.ApellidoPaterno == null)
            {
                _ = objCatalogoDeClientesFormales.ApellidoPaterno == "";
            }

            if (objCatalogoDeClientesFormales.ApellidoMaterno == null)
            {
                _ = objCatalogoDeClientesFormales.ApellidoMaterno == "";
            }

            if (objCatalogoDeClientesFormales.RFC == null)
            {
                _ = objCatalogoDeClientesFormales.RFC == "";
            }

            if (objCatalogoDeClientesFormales.CURP == null)
            {
                _ = objCatalogoDeClientesFormales.CURP == "";
            }

            if (objCatalogoDeClientesFormales.Telefono == null)
            {
                _ = objCatalogoDeClientesFormales.Telefono == "";
            }

            if (objCatalogoDeClientesFormales.EmailContacto == null)
            {
                _ = objCatalogoDeClientesFormales.EmailContacto == "";
            }

            if (objCatalogoDeClientesFormales.Atencion == null)
            {
                _ = objCatalogoDeClientesFormales.Atencion == "";
            }

            if (objCatalogoDeClientesFormales.eMailCFD == null)
            {
                _ = objCatalogoDeClientesFormales.eMailCFD == "";
            }

            if (objCatalogoDeClientesFormales.SectorComercial == null)
            {
                _ = objCatalogoDeClientesFormales.SectorComercial == "";
            }

            if (objCatalogoDeClientesFormales.EmailPdfCASA == null)
            {
                _ = objCatalogoDeClientesFormales.EmailPdfCASA == "";
            }

            if (objCatalogoDeClientesFormales.RfcParaConsulta == null)
            {
                _ = objCatalogoDeClientesFormales.RfcParaConsulta == "";
            }

            if (objCatalogoDeClientesFormales.EmailManifiesto == null)
            {
                _ = objCatalogoDeClientesFormales.EmailManifiesto == "";
            }

            if (objCatalogoDeClientesFormales.IdEncriptado == null)
            {
                _ = objCatalogoDeClientesFormales.IdEncriptado == "";
            }

            if (objCatalogoDeClientesFormales.PasswordWEB == null)
            {
                _ = objCatalogoDeClientesFormales.PasswordWEB == "";
            }

            if (objCatalogoDeClientesFormales.RegimenCapital == null)
            {
                _ = objCatalogoDeClientesFormales.RegimenCapital == "";
            }

            if (objCatalogoDeClientesFormales.idCIF == null)
            {
                _ = objCatalogoDeClientesFormales.idCIF == "";
            }

            if (objCatalogoDeClientesFormales.Ultimo == null)
            {
                _ = objCatalogoDeClientesFormales.Ultimo == "";
            }

            if (objCatalogoDeClientesFormales.Primero == null)
            {
                _ = objCatalogoDeClientesFormales.Primero == "";
            }

            if (objCatalogoDeClientesFormales.Siguiente == null)
            {
                _ = objCatalogoDeClientesFormales.Siguiente == "";
            }

            if (objCatalogoDeClientesFormales.Anterior == null)
            {
                _ = objCatalogoDeClientesFormales.Anterior == "";
            }

            if (objCatalogoDeClientesFormales.DireccionesDeClientes.Poblacion == null)
            {
                _ = objCatalogoDeClientesFormales.DireccionesDeClientes.Poblacion == "";
            }

            if (objCatalogoDeClientesFormales.DireccionesDeClientes.NumeroInt == null)
            {
                _ = objCatalogoDeClientesFormales.DireccionesDeClientes.NumeroInt == "";
            }

            if (objCatalogoDeClientesFormales.DireccionesDeClientes.EntreLaCalleDe == null)
            {
                _ = objCatalogoDeClientesFormales.DireccionesDeClientes.EntreLaCalleDe == "";
            }

            if (objCatalogoDeClientesFormales.DireccionesDeClientes.YDe == null)
            {
                _ = objCatalogoDeClientesFormales.DireccionesDeClientes.YDe == "";
            }

            if (objCatalogoDeClientesFormales.DireccionesDeClientes.Localidad == null)
            {
                _ = objCatalogoDeClientesFormales.DireccionesDeClientes.Localidad == "";
            }

            if (objCatalogoDeClientesFormales.RFC == "" && objCatalogoDeClientesFormales.CURP == "")
            {
                //Error = "Estimado usuario es necesario ingresar RFC o CURP, favor de verificarlo antes de continuar. ";
                //return Error;
                throw new Exception("Estimado usuario es necesario ingresar RFC o CURP, favor de verificarlo antes de continuar. ");
            }

            //if (objCatalogoDeClientesFormales.RFC != "" && objCatalogoDeClientesFormales.CURP != "")
            //{
            //    //Error = "Estimado usuario solo puede ingresar RFC o CURP, favor de verificarlo antes de continuar. ";
            //    //return Error;
            //    throw new Exception("Estimado usuario solo puede ingresar RFC o CURP, favor de verificarlo antes de continuar. ");
            //}

            if (!string.IsNullOrWhiteSpace(objCatalogoDeClientesFormales.RFC) && !string.IsNullOrWhiteSpace(objCatalogoDeClientesFormales.CURP))
            {
                throw new Exception("Estimado usuario, solo puede ingresar RFC o CURP, favor de verificarlo antes de continuar.");
            }

            if (esInsertar == true)
            {
                if (objCatalogoDeClientesFormales.RFC != "")
                {
                    int Existe;
                    var objClientesFormales = new CatalogoDeClientesFormalesRepository(_configuration);
                    Existe = objClientesFormales.ValidaRFC(objCatalogoDeClientesFormales.RFC);
                    if (Existe >= 1)
                    {
                        //Error = $"Estimado usuario el RFC {objCatalogoDeClientesFormales.RFC} que usted está intentando ingresar ya existe en la base de datos y por esta causa no es posible ingresarlo nuevamente, favor de verificarlo antes de continuar. ";
                        //return Error;
                        throw new Exception($"Estimado usuario el RFC {objCatalogoDeClientesFormales.RFC} que usted está intentando ingresar ya existe en la base de datos y por esta causa no es posible ingresarlo nuevamente, favor de verificarlo antes de continuar. ");
                    }

                    if (objCatalogoDeClientesFormales.RFC == "EDM930614781")
                    {
                        //Error = "Estimado usuario, el RFC EDM930614781 no puede ser usado desde esta pantalla, por favor utilice la opción de Clientes de mensajería. ";
                        //return Error;
                        throw new Exception("Estimado usuario, el RFC EDM930614781 no puede ser usado desde esta pantalla, por favor utilice la opción de Clientes de mensajería. ");
                    }

                    Regex pattern_pm = new(@"^(([A-Z�&]{3})([0-9]{2})([0][13578]|[1][02])(([0][1-9]|[12][\\d])|[3][01])([A-Z0-9]{3}))|" +
                      "(([A-Z�&]{3})([0-9]{2})([0][13456789]|[1][012])(([0][1-9]|[12][\\d])|[3][0])([A-Z0-9]{3}))|" +
                      "(([A-Z�&]{3})([02468][048]|[13579][26])[0][2]([0][1-9]|[12][\\d])([A-Z0-9]{3}))|" +
                      "(([A-Z�&]{3})([0-9]{2})[0][2]([0][1-9]|[1][0-9]|[2][0-8])([A-Z0-9]{3}))$");

                    Regex pattern_pf = new(@"^(([A-Z�&]{4})([0-9]{2})([0][13578]|[1][02])(([0][1-9]|[12][\\d])|[3][01])([A-Z0-9]{3}))|" +
                        "(([A-Z�&]{4})([0-9]{2})([0][13456789]|[1][012])(([0][1-9]|[12][\\d])|[3][0])([A-Z0-9]{3}))|" +
                        "(([A-Z�&]{4})([02468][048]|[13579][26])[0][2]([0][1-9]|[12][\\d])([A-Z0-9]{3}))|" +
                        "(([A-Z�&]{4})([0-9]{2})[0][2]([0][1-9]|[1][0-9]|[2][0-8])([A-Z0-9]{3}))$");

                    if (pattern_pm.IsMatch(objCatalogoDeClientesFormales.RFC) || pattern_pf.IsMatch(objCatalogoDeClientesFormales.RFC))
                    {
                        //Error = "";
                    }
                    else
                    {
                        //Error = $"Estimado usuario, el RFC {objCatalogoDeClientesFormales.RFC} no cumple con las características establecidas por el SAT. ";
                        //return Error;
                        throw new Exception($"Estimado usuario, el RFC {objCatalogoDeClientesFormales.RFC} no cumple con las características establecidas por el SAT. ");
                    }
                }

                if (objCatalogoDeClientesFormales.CURP != "")
                {
                    Regex rCurp = new(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$");
                    if (!rCurp.IsMatch(objCatalogoDeClientesFormales.CURP))
                    {
                        //Error = $"Estimado usuario, el CURP {objCatalogoDeClientesFormales.CURP} no cumple con las características establecidas por el SAT. ";
                        //return Error;
                        throw new Exception($"Estimado usuario, el CURP {objCatalogoDeClientesFormales.CURP} no cumple con las características establecidas por el SAT. ");
                    }
                    int Existe;
                    var objClientesFormales = new CatalogoDeClientesFormalesRepository(_configuration);
                    Existe = objClientesFormales.ValidaCURP(objCatalogoDeClientesFormales.CURP);
                    if (Existe >= 1)
                    {
                        //Error = $"Estimado usuario el RFC {objCatalogoDeClientesFormales.CURP} que usted está intentando ingresar ya existe en la base de datos y por esta causa no es posible ingresarlo nuevamente, favor de verificarlo antes de continuar. ";
                        //return Error;
                        throw new Exception($"Estimado usuario el CURP {objCatalogoDeClientesFormales.CURP} que usted está intentando ingresar ya existe en la base de datos y por esta causa no es posible ingresarlo nuevamente, favor de verificarlo antes de continuar. ");
                    }
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.Direccion))
                {
                    throw new Exception("Ingrese una calle");
                }


                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.Colonia))
                {
                    throw new Exception("Municipio/Delegación/Población");
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.CodigoPostal))
                {
                    throw new Exception("Ingrese un código postal");
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.NumeroExt))
                {
                    throw new Exception("Ingrese un número exterior");
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.NumeroInt))
                {
                    objCatalogoDeClientesFormales.DireccionesDeClientes.NumeroInt = "";
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.EntreLaCalleDe))
                {
                    objCatalogoDeClientesFormales.DireccionesDeClientes.EntreLaCalleDe = "";
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.YDe))
                {
                    objCatalogoDeClientesFormales.DireccionesDeClientes.YDe = "";
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.Entidad))
                {
                    throw new Exception("Ingrese una entidad federativa");
                }

                //Se especifica que el usuario puede ingresar la localidad si lo desea, es opcional.
                //if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.DireccionesDeClientes.Localidad))
                //{
                //    throw new Exception("Ingrese una Localidad");
                //}

            }

            if (esInsertar == false)
            {
                if (objCatalogoDeClientesFormales.RFC != "")
                {
                    if (objCatalogoDeClientesFormales.RFC == "EDM930614781")
                    {
                        throw new Exception("Estimado usuario, el RFC EDM930614781 no puede ser usado desde esta pantalla, por favor utilice la opción de Clientes de mensajería. ");

                    }

                    Regex pattern_pm = new(@"^(([A-Z�&]{3})([0-9]{2})([0][13578]|[1][02])(([0][1-9]|[12][\\d])|[3][01])([A-Z0-9]{3}))|" +
                      "(([A-Z�&]{3})([0-9]{2})([0][13456789]|[1][012])(([0][1-9]|[12][\\d])|[3][0])([A-Z0-9]{3}))|" +
                      "(([A-Z�&]{3})([02468][048]|[13579][26])[0][2]([0][1-9]|[12][\\d])([A-Z0-9]{3}))|" +
                      "(([A-Z�&]{3})([0-9]{2})[0][2]([0][1-9]|[1][0-9]|[2][0-8])([A-Z0-9]{3}))$");

                    Regex pattern_pf = new(@"^(([A-Z�&]{4})([0-9]{2})([0][13578]|[1][02])(([0][1-9]|[12][\\d])|[3][01])([A-Z0-9]{3}))|" +
                        "(([A-Z�&]{4})([0-9]{2})([0][13456789]|[1][012])(([0][1-9]|[12][\\d])|[3][0])([A-Z0-9]{3}))|" +
                        "(([A-Z�&]{4})([02468][048]|[13579][26])[0][2]([0][1-9]|[12][\\d])([A-Z0-9]{3}))|" +
                        "(([A-Z�&]{4})([0-9]{2})[0][2]([0][1-9]|[1][0-9]|[2][0-8])([A-Z0-9]{3}))$");

                    if (pattern_pm.IsMatch(objCatalogoDeClientesFormales.RFC) || pattern_pf.IsMatch(objCatalogoDeClientesFormales.RFC))
                    {
                        //Error = "";
                    }
                    else
                    {
                        throw new Exception($"Estimado usuario, el RFC {objCatalogoDeClientesFormales.RFC} no cumple con las características establecidas por el SAT. ");
                    }
                }

                if (objCatalogoDeClientesFormales.CURP != "")
                {
                    Regex rCurp = new(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$");
                    if (!rCurp.IsMatch(objCatalogoDeClientesFormales.CURP))
                    {
                        throw new Exception($"Estimado usuario, el CURP {objCatalogoDeClientesFormales.CURP} no cumple con las características establecidas por el SAT. ");
                    }
                }

                if (string.IsNullOrEmpty(objCatalogoDeClientesFormales.Observaciones))
                {
                    throw new Exception("Estimado usuario falta escribir una observación");
                }

                if (objCatalogoDeClientesFormales.Clave == null)
                {
                    throw new Exception($"El Cliente formal con clave = {objCatalogoDeClientesFormales.Clave} no existe.");
                }
            }
        }
        

        public bool ModificarTipodeFigura(int idCliente, int IDTipodefigura)
        {
            bool Respuesta = false;
            int id;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CATALOGODECLIENTESEXPERTTI_FIGURA", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = idCliente;
                    cmd.Parameters.Add("@TipoDeFigura", SqlDbType.Int, 4).Value = IDTipodefigura;
                    cmd.Parameters.Add("@NEWID_REGISTRO", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value) != -1)
                        {
                            Respuesta = true;
                            id = Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value);
                        }
                        else
                        {
                            Respuesta = false;
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Respuesta;
        }

        public List<CatalogoDeClientesFormales> CargarEmailManifiesto()
        {
            List<CatalogoDeClientesFormales> lstCliente = new();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            try
            {
                //cn.ConnectionString = MyConnectionString;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CLIENTES_CON_PREALERTA_DE_MANIFIESTO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CatalogoDeClientesFormales objCliente = new();
                        objCliente.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objCliente.Clave = dr["Clave"].ToString();
                        objCliente.Nombre = dr["Nombre"].ToString();
                        objCliente.ApellidoPaterno = dr["ApellidoPaterno"].ToString();
                        objCliente.ApellidoMaterno = dr["ApellidoMaterno"].ToString();
                        objCliente.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                        objCliente.RFC = dr["RFC"].ToString();
                        objCliente.CURP = dr["CURP"].ToString();
                        objCliente.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                        objCliente.Activo = Convert.ToBoolean(dr["Activo"]);
                        objCliente.Telefono = dr["Telefono"].ToString();
                        objCliente.EmailContacto = dr["EmailContacto"].ToString();
                        objCliente.Atencion = dr["Atencion"].ToString();
                        objCliente.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                        objCliente.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                        objCliente.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                        objCliente.eMailCFD = dr["eMailCFD"].ToString();
                        objCliente.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                        objCliente.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                        objCliente.SectorComercial = dr["SectorComercial"].ToString();
                        objCliente.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                        objCliente.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                        objCliente.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                        objCliente.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                        objCliente.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                        objCliente.EmailPdfCASA = dr["EmailPdfCASA"].ToString();
                        objCliente.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        objCliente.RfcParaConsulta = dr["RfcParaConsulta"].ToString();
                        objCliente.EmailManifiesto = dr["EmailManifiesto"].ToString();
                        objCliente.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                        //objCliente.SoloDeGrupoEI = Convert.ToBoolean(dr["SoloDeGrupoEI"]);
                        objCliente.SoloDeGrupoEI = Convert.ToInt32(dr["SoloDeGrupoEI"]);
                        objCliente.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                        objCliente.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                        objCliente.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                        objCliente.idCIF = dr["idCIF"].ToString();
                        objCliente.Prospecto = Convert.ToBoolean(dr["Prospecto"]);                       
                        objCliente.DireccionesDeClientes.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                        objCliente.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objCliente.DireccionesDeClientes.Direccion = dr["Direccion"].ToString();
                        objCliente.DireccionesDeClientes.Colonia = dr["Colonia"].ToString();
                        objCliente.DireccionesDeClientes.Poblacion = dr["Poblacion"].ToString();
                        objCliente.DireccionesDeClientes.CodigoPostal = dr["CodigoPostal"].ToString();
                        objCliente.DireccionesDeClientes.NumeroExt = dr["NumeroExt"].ToString();
                        objCliente.DireccionesDeClientes.NumeroInt = dr["NumeroInt"].ToString();
                        objCliente.DireccionesDeClientes.EntreLaCalleDe = dr["EntreLaCalleDe"].ToString();
                        objCliente.DireccionesDeClientes.YDe = dr["YDe"].ToString();
                        objCliente.DireccionesDeClientes.Entidad = dr["ClaveEntidadFederativa"].ToString();
                      

                        lstCliente.Add(objCliente);
                    }
                }
                else
                    lstCliente = null;
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

            return lstCliente;
        }
    }
}
