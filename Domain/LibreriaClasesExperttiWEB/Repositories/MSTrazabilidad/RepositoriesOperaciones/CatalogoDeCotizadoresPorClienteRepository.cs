using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class CatalogoDeCotizadoresPorClienteRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeCotizadoresPorClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CatalogoDeCotizadoresPorCliente> Cargar()
        {
            List<CatalogoDeCotizadoresPorCliente> lstCatalogoDeCotizadoresPorCliente = new List<CatalogoDeCotizadoresPorCliente>();

            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;


            //cn.ConnectionString = myConnectionString;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CatalogoDeCotizadoresPorCliente";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CatalogoDeCotizadoresPorCliente objCatalogoDeCotizadoresPorCliente = new CatalogoDeCotizadoresPorCliente();

                        objCatalogoDeCotizadoresPorCliente.Nombre = dr["Nombre"].ToString().Trim();
                        objCatalogoDeCotizadoresPorCliente.Email = dr["Email"].ToString().Trim();
                        objCatalogoDeCotizadoresPorCliente.IdUsuario = Convert.ToInt32(dr["IDUsuario"]);

                        lstCatalogoDeCotizadoresPorCliente.Add(objCatalogoDeCotizadoresPorCliente);
                    }
                }
                else
                    lstCatalogoDeCotizadoresPorCliente = null;
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

            return lstCatalogoDeCotizadoresPorCliente;
        }
        public CatalogoDeCotizadoresPorCliente Buscar(int IDCliente, int IdOficina)
        {

            var objCATALOGODECOTIZADORESPORCLIENTE = new CatalogoDeCotizadoresPorCliente();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODECOTIZADORESPORCLIENTE_NEW";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4);
            param.Value = IDCliente;

            param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
            param.Value = IdOficina;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODECOTIZADORESPORCLIENTE.IDClienteCotizador = Convert.ToInt32(dr["IDClienteCotizador"]);
                    objCATALOGODECOTIZADORESPORCLIENTE.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objCATALOGODECOTIZADORESPORCLIENTE.IdUsuario = Convert.ToInt32(dr["IDUsuario"]);
                }
                else
                {
                    objCATALOGODECOTIZADORESPORCLIENTE = default;
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

            return objCATALOGODECOTIZADORESPORCLIENTE;
        }
    }
}
