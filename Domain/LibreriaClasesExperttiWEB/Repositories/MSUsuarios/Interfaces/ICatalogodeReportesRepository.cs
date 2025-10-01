using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces
{
    public interface ICatalogodeReportesRepository
    {
        string SConexion { get; set; }

        List<CatalogodeReportes> Cargar(int IdUsuario);
    }
}
