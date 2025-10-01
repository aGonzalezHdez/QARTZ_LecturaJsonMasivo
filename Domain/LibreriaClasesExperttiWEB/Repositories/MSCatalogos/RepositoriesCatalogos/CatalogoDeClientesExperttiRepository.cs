using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeClientesExperttiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeClientesExperttiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogoDeClientesExpertti Buscar(int MyIdCliente)
        {
            var objCATALOGODECLIENTESCASA = new CatalogoDeClientesExpertti();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CATALOGODECLIENTESEXPERTTIXID";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IDCliente INT
                @param = cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4);
                @param.Value = MyIdCliente;
                // Insertar Parametro de busqueda

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODECLIENTESCASA.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objCATALOGODECLIENTESCASA.Clave = dr["Clave"].ToString();
                    objCATALOGODECLIENTESCASA.Nombre = dr["Nombre"].ToString();
                    objCATALOGODECLIENTESCASA.ApellidoPaterno = dr["ApellidoPaterno"].ToString();
                    objCATALOGODECLIENTESCASA.ApellidoMaterno = dr["ApellidoMaterno"].ToString();
                    objCATALOGODECLIENTESCASA.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                    objCATALOGODECLIENTESCASA.RFC = dr["RFC"].ToString();
                    objCATALOGODECLIENTESCASA.CURP = dr["CURP"].ToString();
                    objCATALOGODECLIENTESCASA.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                    objCATALOGODECLIENTESCASA.Activo = Convert.ToBoolean(dr["Activo"]);
                    objCATALOGODECLIENTESCASA.Telefono = dr["Telefono"].ToString();
                    objCATALOGODECLIENTESCASA.EmailContacto = dr["EmailContacto"].ToString();
                    objCATALOGODECLIENTESCASA.Atencion = dr["Atencion"].ToString();
                    objCATALOGODECLIENTESCASA.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                    objCATALOGODECLIENTESCASA.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                    objCATALOGODECLIENTESCASA.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                    objCATALOGODECLIENTESCASA.eMailCFD = dr["eMailCFD"].ToString();
                    objCATALOGODECLIENTESCASA.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                    objCATALOGODECLIENTESCASA.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                    objCATALOGODECLIENTESCASA.SectorComercial = dr["SectorComercial"].ToString();
                    objCATALOGODECLIENTESCASA.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                    objCATALOGODECLIENTESCASA.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                    objCATALOGODECLIENTESCASA.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                    objCATALOGODECLIENTESCASA.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                    objCATALOGODECLIENTESCASA.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                    objCATALOGODECLIENTESCASA.EmailPdfCASA = dr["EmailPdfCASA"].ToString();
                    objCATALOGODECLIENTESCASA.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                    objCATALOGODECLIENTESCASA.RfcParaConsulta = dr["RfcParaConsulta"].ToString();
                    objCATALOGODECLIENTESCASA.EmailManifiesto = dr["EmailManifiesto"].ToString();
                    objCATALOGODECLIENTESCASA.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                    objCATALOGODECLIENTESCASA.SoloDeGrupoEI = Convert.ToInt32(dr["SoloDeGrupoEI"]);
                    objCATALOGODECLIENTESCASA.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                    objCATALOGODECLIENTESCASA.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                    objCATALOGODECLIENTESCASA.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                    objCATALOGODECLIENTESCASA.Prospecto = Convert.ToBoolean(dr["Prospecto"]);
                    objCATALOGODECLIENTESCASA.IdUsuarioManif = Convert.ToInt32(dr["IdUsuarioManif"]);
                }
                // objCATALOGODECLIENTESCASA.Ultimo = dr["Ultimo"].ToString
                // objCATALOGODECLIENTESCASA.Primero = dr["Primero"].ToString
                // objCATALOGODECLIENTESCASA.Siguiente = dr["Siguiente"].ToString
                // objCATALOGODECLIENTESCASA.Anterior = dr["Anterior"].ToString
                else
                {
                    objCATALOGODECLIENTESCASA = default;
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

            return objCATALOGODECLIENTESCASA;
        }

        public CatalogoDeClientesExpertti Buscar(string MyClave)
        {

            var objCATALOGODECLIENTESCASA = new CatalogoDeClientesExpertti();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CLIENTES_POR_CLAVE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @Clave VARCHAR(6)
                @param = cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 6);
                @param.Value = MyClave;
                // Insertar Parametro de busqueda

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODECLIENTESCASA.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objCATALOGODECLIENTESCASA.Clave = dr["Clave"].ToString();
                    objCATALOGODECLIENTESCASA.Nombre = dr["Nombre"].ToString();
                    objCATALOGODECLIENTESCASA.ApellidoPaterno = dr["ApellidoPaterno"].ToString();
                    objCATALOGODECLIENTESCASA.ApellidoMaterno = dr["ApellidoMaterno"].ToString();
                    objCATALOGODECLIENTESCASA.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                    objCATALOGODECLIENTESCASA.RFC = dr["RFC"].ToString();
                    objCATALOGODECLIENTESCASA.CURP = dr["CURP"].ToString();
                    objCATALOGODECLIENTESCASA.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                    objCATALOGODECLIENTESCASA.Activo = Convert.ToBoolean(dr["Activo"]);
                    objCATALOGODECLIENTESCASA.Telefono = dr["Telefono"].ToString();
                    objCATALOGODECLIENTESCASA.EmailContacto = dr["EmailContacto"].ToString();
                    objCATALOGODECLIENTESCASA.Atencion = dr["Atencion"].ToString();
                    objCATALOGODECLIENTESCASA.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                    objCATALOGODECLIENTESCASA.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                    objCATALOGODECLIENTESCASA.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                    objCATALOGODECLIENTESCASA.eMailCFD = dr["eMailCFD"].ToString();
                    objCATALOGODECLIENTESCASA.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                    objCATALOGODECLIENTESCASA.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                    objCATALOGODECLIENTESCASA.SectorComercial = dr["SectorComercial"].ToString();
                    objCATALOGODECLIENTESCASA.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                    objCATALOGODECLIENTESCASA.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                    objCATALOGODECLIENTESCASA.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                    objCATALOGODECLIENTESCASA.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                    objCATALOGODECLIENTESCASA.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                    objCATALOGODECLIENTESCASA.EmailPdfCASA = dr["EmailPdfCASA"].ToString();
                    objCATALOGODECLIENTESCASA.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                    objCATALOGODECLIENTESCASA.RfcParaConsulta = dr["RfcParaConsulta"].ToString();
                    objCATALOGODECLIENTESCASA.EmailManifiesto = dr["EmailManifiesto"].ToString();
                    objCATALOGODECLIENTESCASA.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                    objCATALOGODECLIENTESCASA.SoloDeGrupoEI = Convert.ToInt32(dr["SoloDeGrupoEI"]);
                    objCATALOGODECLIENTESCASA.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                    objCATALOGODECLIENTESCASA.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                    objCATALOGODECLIENTESCASA.Ultimo = dr["Ultimo"].ToString();
                    objCATALOGODECLIENTESCASA.Primero = dr["Primero"].ToString();
                    objCATALOGODECLIENTESCASA.Siguiente = dr["Siguiente"].ToString();
                    objCATALOGODECLIENTESCASA.Anterior = dr["Anterior"].ToString();
                    objCATALOGODECLIENTESCASA.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                    objCATALOGODECLIENTESCASA.Prospecto = Convert.ToBoolean(dr["Prospecto"]);
                    objCATALOGODECLIENTESCASA.IdUsuarioManif = Convert.ToInt32(dr["IdUsuarioManif"]);
                }

                else
                {
                    objCATALOGODECLIENTESCASA = default;
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

            return objCATALOGODECLIENTESCASA;
        }

    }
}
