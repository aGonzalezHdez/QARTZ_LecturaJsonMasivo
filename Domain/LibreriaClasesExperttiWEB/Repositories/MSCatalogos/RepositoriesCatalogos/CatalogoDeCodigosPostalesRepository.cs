using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeCodigosPostalesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;
        public CatalogoDeCodigosPostalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public async Task<List<CatalogoDeCodigosPostales>> BuscarCodigosPostPorEntFed(string EntFed)
        {

            List<CatalogoDeCodigosPostales> list = new List<CatalogoDeCodigosPostales>();

            using (cn = new(sConexion))
            using (SqlCommand cmd = new("NET_SEARCH_CATALOGODECODIGOSPOSTALESPORCVEENTIDADFEDERATIVA", cn))
            {
                try
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ENTIDADFEDERATIVA", SqlDbType.VarChar, 50).Value = EntFed;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        list = SqlDataReaderToList.DataReaderMapToList<CatalogoDeCodigosPostales>(reader);
                    }

                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message.ToString() + "NET_SEARCH_CATALOGODECODIGOSPOSTALESPORCVEENTIDADFEDERATIVA");
                    throw new Exception("Ocurrio un error al consultar los codigos postales por Ent. Fed: " + ex.Message);
                }
            }
            return list.ToList();

        }

        public async Task<List<CatalogoDeCodigosPostales>> Buscar(string CodigoPostal)
        {
            List<CatalogoDeCodigosPostales> list = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CODIGOSPOSTALES", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar).Value = CodigoPostal;
                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    list = SqlDataReaderToList.DataReaderMapToList<CatalogoDeCodigosPostales>(reader);


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
    }
}
