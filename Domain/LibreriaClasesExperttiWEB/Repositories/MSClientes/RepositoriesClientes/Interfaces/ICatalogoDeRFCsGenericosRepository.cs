using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeRFCsGenericosRepository
    {
        string SConexion { get; set; }

        List<CatalogoDeRFCsGenericos> Buscar();
       
    }
}