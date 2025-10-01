using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface ICatalogoDeOficinasRepository
    {
        string SConexion { get; set; }

        CatalogoDeOficinas Buscar(int idOficina);
    }
}
