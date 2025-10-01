using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ISubTipoDeDocumentoRepository
    {
        string SConexion { get; set; }

        Task<List<SubTipoDeDocumento>> CargarCombo();
    }
}