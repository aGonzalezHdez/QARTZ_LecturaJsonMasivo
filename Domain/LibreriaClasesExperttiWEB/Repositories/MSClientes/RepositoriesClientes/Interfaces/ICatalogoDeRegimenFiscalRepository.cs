using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeRegimenFiscalRepository
    {
        string SConexion { get; set; }

        List<CatalogoDeRegimenFiscal> Buscar(int TipoDeFigura);
    }
}