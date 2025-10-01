using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface ICatalogodeDestinatariosRepository
    {
        string sConexion { get; set; }

        CatalogodeDestinatarios Buscar(int IdDestinatario);
        int Insertar(CatalogodeDestinatarios objCatalogodeDestinatarios);
        int Modificar(CatalogodeDestinatarios objCatalogodeDestinatarios);
    }
}