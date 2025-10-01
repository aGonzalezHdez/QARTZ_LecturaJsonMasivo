using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoEntidadesFederativasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CatalogoEntidadesFederativasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<List<Ctarc_EntFed>> Cargar(bool iCliente)
        {
            List<Ctarc_EntFed> list = new();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CTARC_ENTFED", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CLIENTE", SqlDbType.Bit).Value = iCliente;

                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    list = SqlDataReaderToList.DataReaderMapToList<Ctarc_EntFed>(reader);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al consultar Entidades Federativas: " + ex.Message);
            }

            return list.ToList();

        }

        public async Task<List<Ctarc_EntFed>> BuscarClave(string CVE_EFED)
        {
            List<Ctarc_EntFed> list = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CTARC_ENTFED_SINPAIS", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CVE_EFED", SqlDbType.VarChar).Value = CVE_EFED;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        list = SqlDataReaderToList.DataReaderMapToList<Ctarc_EntFed>(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list.ToList();
        }
    }
}
