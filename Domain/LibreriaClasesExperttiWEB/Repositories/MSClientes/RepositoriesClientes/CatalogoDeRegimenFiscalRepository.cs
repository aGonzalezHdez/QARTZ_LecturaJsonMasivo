using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeRegimenFiscalRepository : ICatalogoDeRegimenFiscalRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeRegimenFiscalRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeRegimenFiscalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CatalogoDeRegimenFiscal> Buscar(int TipoDeFigura)
        {
            List<CatalogoDeRegimenFiscal> list = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_CATALOGODEREGIMENFISCAL", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TipoDeFigura", SqlDbType.Int).Value = TipoDeFigura;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    list = SqlDataReaderToList.DataReaderMapToList<CatalogoDeRegimenFiscal>(reader);
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
