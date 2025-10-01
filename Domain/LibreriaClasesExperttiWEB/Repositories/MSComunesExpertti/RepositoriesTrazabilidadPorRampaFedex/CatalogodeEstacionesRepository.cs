using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidadPorRampaFedex;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTrazabilidadPorRampaFedex
{
    public class CatalogodeEstacionesRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeEstacionesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public async Task<List<CatalogodeEstaciones>> Cargar(int IdOficina)
        {
            List<CatalogodeEstaciones> lstEstaciones = new();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("[fdx].[NET_LOAD_ESTACIONES_NEW]", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("IdOficina", SqlDbType.Int, 4).Value = IdOficina;

                    using SqlDataReader sdr = await cmd.ExecuteReaderAsync();
                    lstEstaciones = SqlDataReaderToList.DataReaderMapToList<CatalogodeEstaciones>(sdr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstEstaciones;
        }

        public async Task<List<DropDownListDatos>> ReturnCombo(int IdOficina, int IdUsuario)
        {

            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("[fdx].[NET_LOAD_ESTACIONES_WEB]", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }
    }
}
