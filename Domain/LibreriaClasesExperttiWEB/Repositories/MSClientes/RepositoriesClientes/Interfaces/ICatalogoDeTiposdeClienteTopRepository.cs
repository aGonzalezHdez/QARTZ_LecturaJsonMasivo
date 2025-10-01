using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeTiposdeClienteTopRepository
    {
        string SConexion { get; set; }

        List<CatalogoDeTiposdeClienteTop> Cargar();
    }
}