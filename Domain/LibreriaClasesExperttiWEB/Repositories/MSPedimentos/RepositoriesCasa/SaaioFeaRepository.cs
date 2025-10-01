using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioFeaRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public SaaioFeaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(SaaioFea lsaaio_fea)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {
                cmd.CommandText = "NET_INSERT_CASA_SAAIO_FEA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = lsaaio_fea.NUM_REFE == null ? DBNull.Value : lsaaio_fea.NUM_REFE;

                // ,@NUM_FEA  varchar
                @param = cmd.Parameters.Add("@NUM_FEA", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_fea.NUM_FEA == null ? DBNull.Value : lsaaio_fea.NUM_FEA;

                // ,@NOM_ARCH  varchar
                @param = cmd.Parameters.Add("@NOM_ARCH", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_fea.NOM_ARCH == null ? DBNull.Value : lsaaio_fea.NOM_ARCH;

                // ,@CVE_CAPT  varchar
                @param = cmd.Parameters.Add("@CVE_CAPT", SqlDbType.VarChar, 8);
                @param.Value = lsaaio_fea.CVE_CAPT == null ? DBNull.Value : lsaaio_fea.CVE_CAPT;

                // ,@NUM_FEA2  varchar
                @param = cmd.Parameters.Add("@NUM_FEA2", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_fea.NUM_FEA2 == null ? DBNull.Value : lsaaio_fea.NUM_FEA2;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;


                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASA_SAAIO_FEA");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
    }
}
