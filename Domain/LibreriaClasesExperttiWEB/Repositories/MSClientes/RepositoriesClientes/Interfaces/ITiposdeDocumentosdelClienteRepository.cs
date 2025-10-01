using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ITiposdeDocumentosdelClienteRepository
    {
        string SConexion { get; set; }

        TiposdeDocumentosdelCliente Buscar(int IDTiposDeDocumentosDeCliente);
        Task<List<TiposdeDocumentosdelCliente>> CargarCombo(int MyIDCliente);
        void InsertaBitacora(int IdUsuario, int IdCliente, int IdTiposDeDocumentosDeCliente, int TipoDeCambio);
    }
}