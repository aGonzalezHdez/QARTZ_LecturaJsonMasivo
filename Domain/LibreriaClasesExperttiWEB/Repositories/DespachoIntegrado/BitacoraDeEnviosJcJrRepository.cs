using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class BitacoraDeEnviosJcJrRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public BitacoraDeEnviosJcJrRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(string NUM_REFE, int Completo)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.CommandText = "NET_INSERT_CASAEI_BITACORADEENVIOSJCJR_NEW";
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        // @NUM_REFE varchar
                        cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;

                        // @Completo INT 
                        cmd.Parameters.Add("@Completo", SqlDbType.Int).Value = Completo;

                        SqlParameter outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        id = (int)cmd.Parameters["@newid_registro"].Value;
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        throw new Exception(ex.Message + " NET_INSERT_CASAEI_BITACORADEENVIOSJCJR_NEW");
                    }
                }
            }

            return id;
        }
    }
}
