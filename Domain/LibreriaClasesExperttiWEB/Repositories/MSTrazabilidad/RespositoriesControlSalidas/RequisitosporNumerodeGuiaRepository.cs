using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class RequisitosporNumerodeGuiaRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public RequisitosporNumerodeGuiaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Modificar(int IdReferencia, int IdRequisitos, int RequisitoCumplido, int IdUsuarioQueCumple)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_REQUISITOSPORNUMERODEGUIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@IdReferencia  int
                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = IdReferencia;

                // ,@IdRequisitos  int
                @param = cmd.Parameters.Add("@IdRequisitos", SqlDbType.Int, 4);
                @param.Value = IdRequisitos;


                // ,@IdReferencia  int
                @param = cmd.Parameters.Add("@RequisitoCumplido", SqlDbType.Int, 4);
                @param.Value = RequisitoCumplido;

                // ,@IdRequisitos  int
                @param = cmd.Parameters.Add("@IdUsuarioQueCumple", SqlDbType.Int, 4);
                @param.Value = IdUsuarioQueCumple;


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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_REQUISITOSPORNUMERODEGUIA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

    }
}
