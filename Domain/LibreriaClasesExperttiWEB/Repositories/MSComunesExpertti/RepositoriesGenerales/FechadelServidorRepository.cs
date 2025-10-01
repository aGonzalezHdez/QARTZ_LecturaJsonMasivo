using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class FechadelServidorRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public FechadelServidorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public FechadelServidor Buscar()
        {
            var objFechadelServidor = new FechadelServidor();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_FECHAYHORADELSERVIDOR";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objFechadelServidor.anio = dr["anio"].ToString();
                    objFechadelServidor.dia = dr["dia"].ToString();
                    objFechadelServidor.Fecha = Convert.ToDateTime(dr["Fecha"]);
                    objFechadelServidor.Fechayhora = Convert.ToDateTime(dr["Fechayhora"]);
                    objFechadelServidor.hora = dr["Hora"].ToString();
                    objFechadelServidor.mes = dr["mes"].ToString();
                    objFechadelServidor.milisegundo = dr["milisegundo"].ToString();
                    objFechadelServidor.minuto = dr["minuto"].ToString();
                    objFechadelServidor.segundo = dr["segundo"].ToString();
                }
                else
                {
                    objFechadelServidor = default;
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

            return objFechadelServidor;
        }

    }
}
