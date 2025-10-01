using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface ICatalogodePuestosRepository
    {
        string SConexion { get; set; }

        List<DropDownListDatos> CargarPuestos();
        CatalogodePuestos GetPuesto(int IdPuesto);
    }
}
