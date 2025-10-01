using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IDocumentosClientesRepository
    {
        string SConexion { get; set; }

        DocumentosClientes Buscar(int IDDocumentosDelCliente);
        int BuscarConsecutivo(int IDCliente, int IDTiposDeDocumentosDeCliente);
        List<DocumentosClientesGridView> Cargar(int IdCliente);
        List<DocumentosClientes> CargarEncargo(int IDCliente);
        bool Desactivar(int IDDocumentodeCliente);
        int Insertar(DocumentosClientes objDocumentosClientes);
        int Modificar(DocumentosClientes objDocumentosClientes);
        string NombreFile(int IDCliente, int IDDocumentosDelCliente, string file);
        void ValidaObjeto(DocumentosClientes objDocumentosClientes);

        List<int> CargarNoEstanS3();

    }
}