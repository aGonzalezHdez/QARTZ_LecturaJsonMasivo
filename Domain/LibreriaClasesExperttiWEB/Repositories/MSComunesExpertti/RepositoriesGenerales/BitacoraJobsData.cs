using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class BitacoraJobsData : IBitacoraJobsData
    {
        private readonly IConfiguration _configuration;
        public string SConexion { get; set; }


        public BitacoraJobsData(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(BitacoraJobs bitacora)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_BITACORA_JOBS_INSERT";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            //,@NUM_REFE  varchar
            param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 255);
            param.Value = bitacora.Descripcion;

            //,@NUM_PART  varchar
            param = cmd.Parameters.Add("@Job", SqlDbType.VarChar, 255);
            param.Value = bitacora.Job;

            //,@NUM_SERI  varchar
            param = cmd.Parameters.Add("@Error", SqlDbType.Text);
            param.Value = bitacora.ErrorT;

            param = cmd.Parameters.Add("@new_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@new_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "[NET_BITACORA_JOBS_INSERT]");
                // Return 0; // You cannot return here because it's in catch block
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
            return id;
        }
    }
}
