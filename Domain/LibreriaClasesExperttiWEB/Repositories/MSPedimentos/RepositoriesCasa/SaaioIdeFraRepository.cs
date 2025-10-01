using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioIdeFraRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SaaioIdeFraRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int BuscarIdentificadorDePartida(string MyNum_Refe, int MyNUM_PART, string MyCVE_PERM)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SABER_SI_EXISTE_IDENTIFICADOR_DE_PARTIDA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // ,@NUM_PART  float
                @param = cmd.Parameters.Add("@NUM_PART", SqlDbType.Int, 4);
                @param.Value = MyNUM_PART;

                // ,@CVE_PERM varchar(2)
                @param = cmd.Parameters.Add("@CVE_PERM", SqlDbType.VarChar, 2);
                @param.Value = MyCVE_PERM;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;



                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_SABER_SI_EXISTE_IDENTIFICADOR_DE_PARTIDA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public int Insertar(SaaioIdeFra lSaaioIdeFra)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_INSERT_SAAIO_IDEFRA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = lSaaioIdeFra.NUM_REFE == null ? DBNull.Value : lSaaioIdeFra.NUM_REFE;

                // ,@NUM_PART  float
                @param = cmd.Parameters.Add("@NUM_PART", SqlDbType.Float, 4);
                @param.Value = lSaaioIdeFra.NUM_PART == null ? DBNull.Value : lSaaioIdeFra.NUM_PART;

                // ,@NUM_IDE  int
                @param = cmd.Parameters.Add("@NUM_IDE", SqlDbType.Int, 4);
                @param.Value = lSaaioIdeFra.NUM_IDE == null ? DBNull.Value : lSaaioIdeFra.NUM_IDE;

                // ,@CVE_PERM  varchar
                @param = cmd.Parameters.Add("@CVE_PERM", SqlDbType.VarChar, 2);
                @param.Value = lSaaioIdeFra.CVE_PERM == null ? DBNull.Value : lSaaioIdeFra.CVE_PERM;

                // ,@FEC_PERM  datetime
                @param = cmd.Parameters.Add("@FEC_PERM", SqlDbType.DateTime, 4);
                @param.Value = DBNull.Value;

                // ,@NUM_PERM2  varchar
                @param = cmd.Parameters.Add("@NUM_PERM2", SqlDbType.VarChar, 50);
                @param.Value = lSaaioIdeFra.NUM_PERM2 == null ? DBNull.Value : lSaaioIdeFra.NUM_PERM2;

                // ,@NUM_PERM  varchar
                @param = cmd.Parameters.Add("@NUM_PERM", SqlDbType.VarChar, 20);
                @param.Value = lSaaioIdeFra.NUM_PERM == null ? DBNull.Value : lSaaioIdeFra.NUM_PERM;

                // ,@NUM_PERM3  varchar
                @param = cmd.Parameters.Add("@NUM_PERM3", SqlDbType.VarChar, 40);
                @param.Value = lSaaioIdeFra.NUM_PERM3 == null ? DBNull.Value : lSaaioIdeFra.NUM_PERM3;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;



                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_IDEFRA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

    }
}
