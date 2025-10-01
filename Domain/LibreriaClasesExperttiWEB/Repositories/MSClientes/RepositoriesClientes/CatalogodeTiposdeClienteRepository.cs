using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeTiposDeClienteRepository : ICatalogoDeTiposDeClienteRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeTiposDeClienteRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeTiposDeClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<CatalogoDeTiposDeCliente> Cargar()
        {
            List<CatalogoDeTiposDeCliente> listTiposDeCliente = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODETIPOSDECLIENTE", con))

                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    listTiposDeCliente = SqlDataReaderToList.DataReaderMapToList<CatalogoDeTiposDeCliente>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listTiposDeCliente;
        }
    }
}
