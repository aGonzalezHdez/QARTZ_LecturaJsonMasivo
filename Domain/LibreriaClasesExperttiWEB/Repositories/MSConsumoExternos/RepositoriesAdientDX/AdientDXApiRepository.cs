using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.AdientDXSACI;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesAdientDX
{
    public class AdientDXApiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;

        public AdientDXApiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public AdientDXApi? Buscar()
        {
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_CASAEI_ADIENTDXAPI", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new AdientDXApi
                    {
                        Uri = dr["Uri"].ToString(),
                        Token = dr["Token"].ToString(),
                        UserId = dr["UserId"].ToString(),
                        Receiver = dr["Receiver"].ToString()
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar configuración AdientDXApi: " + ex.Message, ex);
            }
        }
    }
}
