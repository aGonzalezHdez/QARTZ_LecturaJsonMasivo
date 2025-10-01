using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface IDatosDeLaEmpresaRepository
    {
        string SConexion { get; set; }

        DatosDeLaEmpresa Buscar(int id);

        List<DropDownListDatos> Cargar();

    }
}
