using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesConsultasWsExternos;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesConsultasWsExternos
{
    public class SDTConsultaWecPaletsRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SDTConsultaWecPaletsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(SDTConsultaWecPalets lsdtconsultawecpalets)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cmd.CommandText = "NET_INSERT_CASAEI_SDTConsultaWecPalets_New";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@Consecutivo  int
                param = cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4);
                param.Value = lsdtconsultawecpalets.Consecutivo;

                // ,@GuiaHouse  varchar
                param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 10);
                param.Value = lsdtconsultawecpalets.GuiaHouse;

                // ,@GuiaMaster  varchar
                param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawecpalets.GuiaMaster;

                // ,@Bultos  int
                param = cmd.Parameters.Add("@Bultos", SqlDbType.Int, 4);
                param.Value = lsdtconsultawecpalets.Bultos;

                // ,@Peso  decimal
                param = cmd.Parameters.Add("@Peso", SqlDbType.Decimal, 4);
                param.Value = lsdtconsultawecpalets.Peso;

                // ,@Salidas  bit
                param = cmd.Parameters.Add("@Salidas", SqlDbType.Bit, 4);
                param.Value = lsdtconsultawecpalets.Salidas;

                // ,@Ubicacion  varchar
                param = cmd.Parameters.Add("@Ubicacion", SqlDbType.VarChar, 50);
                param.Value = lsdtconsultawecpalets.Ubicacion;

                param = cmd.Parameters.Add("@IdWec", SqlDbType.Int, 4);
                param.Value = lsdtconsultawecpalets.IdWec;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;


                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_SDTConsultaWecPalets");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

    }
}
