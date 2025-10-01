using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales.Interfaces
{
    public interface IBitacoraJobsData
    {
        string SConexion { get; set; }

        int Insertar(BitacoraJobs bitacora);
    }
}