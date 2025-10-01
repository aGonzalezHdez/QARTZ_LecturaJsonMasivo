using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeAgentesAduanalesRepository
    {
        string SConexion { get; set; }

        List<CatalogoDeAgentesAduanales> Buscar(string Patente);

        CatalogoDeAgentesAduanales BuscarRFC(string RFC);

    }
}