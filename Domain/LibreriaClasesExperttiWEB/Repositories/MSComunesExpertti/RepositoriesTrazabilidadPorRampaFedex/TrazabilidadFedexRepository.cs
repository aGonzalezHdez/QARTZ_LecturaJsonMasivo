using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTrazabilidadPorRampaFedex
{
    public class TrazabilidadFedexRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public TrazabilidadFedexRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<DataTable> Cargar(int Ide, int IdOficina, int IdEstacion, DateTime Fecha)
        {
            DataTable dt = new();

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_REPORT_TRAZABILIDADXPO_FEDEX_WEB", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Ide", SqlDbType.Int).Value = Ide;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@IdEstacion ", SqlDbType.Int, 4).Value = IdEstacion;
                    cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Fecha;

                    using SqlDataAdapter da = new(cmd);
                    await Task.Run(() => da.Fill(dt));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_REPORT_TRAZABILIDADXPO_FEDEX_WEB");
            }

            return dt;
        }
    }
}
