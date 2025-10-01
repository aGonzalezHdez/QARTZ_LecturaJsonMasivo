using LibreriaClasesAPIExpertti.Utilities.Converters;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class AltaDigitalRepository : IAltaDigitalRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public AltaDigitalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeClientesFormales GetSolicitudPorRFC(string RFC)
        {
            CatalogoDeClientesFormales? SoliciudporRFC = new();
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_INFOALTACLIENTEDIGITAL_GENERALES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = RFC;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();

                        SoliciudporRFC.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        SoliciudporRFC.Clave = string.Format("{0}", dr["Clave"]);
                        SoliciudporRFC.Nombre = string.Format("{0}", dr["Nombre"]);
                        SoliciudporRFC.ApellidoPaterno = string.Format("{0}", dr["ApellidoPaterno"]);
                        SoliciudporRFC.ApellidoMaterno = string.Format("{0}", dr["ApellidoMaterno"]);
                        SoliciudporRFC.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                        SoliciudporRFC.RFC = string.Format("{0}", dr["RFC"]);
                        SoliciudporRFC.CURP = string.Format("{0}", dr["CURP"]);
                        SoliciudporRFC.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                        SoliciudporRFC.Activo = Convert.ToBoolean(dr["Activo"]);
                        SoliciudporRFC.Telefono = string.Format("{0}", dr["Telefono"]);
                        SoliciudporRFC.EmailContacto = string.Format("{0}", dr["EmailContacto"]);
                        SoliciudporRFC.Atencion = string.Format("{0}", dr["Atencion"]);
                        SoliciudporRFC.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                        SoliciudporRFC.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                        SoliciudporRFC.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                        SoliciudporRFC.eMailCFD = string.Format("{0}", dr["eMailCFD"]);
                        SoliciudporRFC.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                        SoliciudporRFC.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                        SoliciudporRFC.SectorComercial = string.Format("{0}", dr["SectorComercial"]);
                        SoliciudporRFC.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                        SoliciudporRFC.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                        SoliciudporRFC.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                        SoliciudporRFC.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                        SoliciudporRFC.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                        SoliciudporRFC.EmailPdfCASA = string.Format("{0}", dr["EmailPdfCASA"]);
                        SoliciudporRFC.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        SoliciudporRFC.RfcParaConsulta = string.Format("{0}", dr["RfcParaConsulta"]);
                        SoliciudporRFC.EmailManifiesto = string.Format("{0}", dr["EmailManifiesto"]);
                        SoliciudporRFC.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                        SoliciudporRFC.SoloDeGrupoEI = Convert.ToInt32(dr["SoloDeGrupoEI"]);
                        SoliciudporRFC.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                        SoliciudporRFC.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                        SoliciudporRFC.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                        SoliciudporRFC.Prospecto = Convert.ToBoolean(dr["Prospecto"]);
                        SoliciudporRFC.IdUsuarioManif = Convert.ToInt32(dr["IdUsuarioManif"]);
                        SoliciudporRFC.IDTipoCliente = Convert.ToInt32(dr["IDTipoCliente"]);
                        SoliciudporRFC.IDTipoClienteTop = Convert.ToInt32(dr["IDTipoClienteTop"]);
                        SoliciudporRFC.IdRegimenFiscal = Convert.ToInt32(dr["IdRegimenFiscal"]);
                        SoliciudporRFC.RegimenCapital = string.Format("{0}", dr["RegimenCapital"]);
                        SoliciudporRFC.Ultimo = string.Format("{0}", dr["Ultimo"]);
                        SoliciudporRFC.Primero = string.Format("{0}", dr["Primero"]);
                        SoliciudporRFC.Siguiente = string.Format("{0}", dr["Siguiente"]);
                        SoliciudporRFC.Anterior = string.Format("{0}", dr["Anterior"]);

                        DireccionesDeClientes objDireccion = new();
                        int idCliente = 0;
                        objDireccion.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                        objDireccion.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objDireccion.Direccion = string.Format("{0}", dr["Direccion"]);
                        objDireccion.Colonia = string.Format("{0}", dr["Colonia"]);
                        objDireccion.Poblacion = string.Format("{0}", dr["Poblacion"]);
                        objDireccion.CodigoPostal = string.Format("{0}", dr["CodigoPostal"]);
                        objDireccion.NumeroExt = string.Format("{0}", dr["NumeroExt"]);
                        objDireccion.NumeroInt = string.Format("{0}", dr["NumeroInt"]);
                        objDireccion.EntreLaCalleDe = string.Format("{0}", dr["EntreLaCalleDe"]);
                        objDireccion.YDe = string.Format("{0}", dr["YDe"]);
                        objDireccion.Entidad = string.Format("{0}", dr["Entidad"]);
                        objDireccion.Activo = Convert.ToBoolean(dr["Activo"]);
                        objDireccion.Orden = Convert.ToInt32(dr["Orden"]);
                        objDireccion.Localidad = string.Format("{0}", dr["Localidad"]);
                        SoliciudporRFC.DireccionesDeClientes = objDireccion;
                    }
                    else {
                        SoliciudporRFC = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return SoliciudporRFC;
        }
    }
}
