using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    public interface IUbicaciondeArchivosRepository
    {
        string SConexion { get; set; }
        UbicaciondeArchivos Buscar(int IdUbicacion);

        string fMisDocumentos(int idUsuario);
    }
}
