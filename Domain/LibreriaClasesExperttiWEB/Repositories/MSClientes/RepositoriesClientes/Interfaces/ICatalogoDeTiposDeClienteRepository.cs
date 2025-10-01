using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeTiposDeClienteRepository
    {
        string SConexion { get; set; }

        List<CatalogoDeTiposDeCliente> Cargar();
    }
}