using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class DatosDodaRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public DatosDodaRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


    }
}
