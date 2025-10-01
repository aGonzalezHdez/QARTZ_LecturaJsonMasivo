using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces
{
    public interface ITipodeMercanciasRepository
    {
        string SConexion { get; set; }

        List<DropDownListDatos> Load();
    }
}
