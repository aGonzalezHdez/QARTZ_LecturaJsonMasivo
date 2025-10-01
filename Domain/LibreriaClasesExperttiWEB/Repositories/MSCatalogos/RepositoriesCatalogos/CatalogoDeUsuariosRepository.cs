using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeUsuarios2Repository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeUsuarios2Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public void UpdateGuiaBaby(string GuiaHouse, DateTime FechaSalida)
        {
            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                using (SqlCommand cmd = new SqlCommand("Pocket.NET_UPDATE_GUIABABY", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 15).Value = GuiaHouse;
                        cmd.Parameters.Add("@FechaSalida", SqlDbType.DateTime, 4).Value = FechaSalida;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + " Pocket.NET_UPDATE_GUIABABY");
                    }
                }
            }
        }

    }
}
