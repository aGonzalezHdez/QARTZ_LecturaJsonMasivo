using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using System.Data;
using System.Data.SqlClient;
namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioContenRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public SaaioContenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<CContenedores> CargarporPredoda(int idPredoda)
        {
            List<CContenedores> listado = new List<CContenedores>() ;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;
            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CONTENEDORES_PORPREDODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                param = cmd.Parameters.Add("@IdPredoda", SqlDbType.Int, 4);
                param.Value = idPredoda;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        listado.Add(new CContenedores
                        {
                            _ValorContenedor = dr["NUM_CONT"].ToString()
                    });
                    }
                    
                }
                
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString()+ " NET_LOAD_CONTENEDORES_PORPREDODA");
            }

            return listado;
        }
    }
}
