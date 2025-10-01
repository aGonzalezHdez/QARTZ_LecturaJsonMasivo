using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeTiposdeClienteTopRepository : ICatalogoDeTiposdeClienteTopRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeTiposdeClienteTopRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeTiposdeClienteTopRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CatalogoDeTiposdeClienteTop> Cargar()
        {
            List<CatalogoDeTiposdeClienteTop> listClienteTop = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_LOAD_CATALOGODETIPOSDECLIENTETOP", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    listClienteTop = SqlDataReaderToList.DataReaderMapToList<CatalogoDeTiposdeClienteTop>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listClienteTop;
        }

    }
}
