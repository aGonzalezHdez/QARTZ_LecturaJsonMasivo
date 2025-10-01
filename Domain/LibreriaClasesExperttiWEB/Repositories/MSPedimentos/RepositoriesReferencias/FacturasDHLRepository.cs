using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias
{
    public class FacturasDHLRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        public FacturasDHLRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public bool Insertar(string MyPedimentos, string MyPatente, int MyNumeroDeLote, string MiAduana)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand();
                SqlParameter param;

                try
                {
                    cn.ConnectionString = sConexion;
                    cn.Open();
                    cmd.CommandText = "NET_INSERT_FACTURASDHL_NEW";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // @ArrayPedimentos VARCHAR(MAX)
                    param = cmd.Parameters.Add("@ArrayPedimentos", SqlDbType.VarChar, 8000);
                    param.Value = MyPedimentos;

                    // @Patente VARCHAR(4)
                    param = cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4);
                    param.Value = MyPatente;

                    // @NumeroDeLote INT
                    param = cmd.Parameters.Add("@NumeroDeLote", SqlDbType.Int, 4);
                    param.Value = MyNumeroDeLote;

                    // @Aduana VARCHAR(3)
                    param = cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3);
                    param.Value = MiAduana;

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "NET_INSERT_FACTURASDHL_NEW");
                }
                finally
                {
                    cn.Close();
                    cn.Dispose();
                }
            }
        }

    }
}
