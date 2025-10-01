using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeCategoriasRepository
    {
        string SConexion { get; set; }

        List<CatalogoDeCategorias> Cargar();
    }
}