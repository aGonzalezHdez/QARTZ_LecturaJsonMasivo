using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces
{
    public interface IBitacoraJsonCMFRepository
    {
        string sConexion { get; set; }

        BitacoraJsonCMF BuscarActivos();
        int Insertar();
        int Modificar(BitacoraJsonCMF objBitacoraJsonCMF);
    }
}