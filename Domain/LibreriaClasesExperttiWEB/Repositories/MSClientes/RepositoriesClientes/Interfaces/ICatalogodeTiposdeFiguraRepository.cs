using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogodeTiposdeFiguraRepository
    {
        string SConexion { get; set; }

        List<DropDownListDatos> Cargar();
    }
}