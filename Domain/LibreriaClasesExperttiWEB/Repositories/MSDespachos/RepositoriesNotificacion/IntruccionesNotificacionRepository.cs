using LibreriaClasesAPIExpertti.Entities.EntitiesNotificacion;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesNotificacion
{
    public class IntruccionesNotificacionRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public IntruccionesNotificacionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public IntruccionesNotificacion Buscar(int IdReferencia)
        {
            var objINSTRUCCIONESDELNOTIFICADOR = new IntruccionesNotificacion();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_INSTRUCCIONESDELNOTIFICADOR";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            param.Value = IdReferencia;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objINSTRUCCIONESDELNOTIFICADOR.IdInstrucciones = Convert.ToInt32(dr["IdInstrucciones"]);
                    objINSTRUCCIONESDELNOTIFICADOR.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                    objINSTRUCCIONESDELNOTIFICADOR.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                    objINSTRUCCIONESDELNOTIFICADOR.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                    objINSTRUCCIONESDELNOTIFICADOR.CveProv = dr["CveProv"].ToString();
                    objINSTRUCCIONESDELNOTIFICADOR.Regimen = dr["Regimen"].ToString();
                    objINSTRUCCIONESDELNOTIFICADOR.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                }
                else
                {
                    objINSTRUCCIONESDELNOTIFICADOR = default;
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

            return objINSTRUCCIONESDELNOTIFICADOR;
        }

    }
}
