using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogodeDocumentosDeProductosRepository
    {
        string SConexion { get; set; }

        int BuscarConsecutivo(int IdCliente, string CodigoDeProducto);

        List<CargarImagenes> Cargar(int IdCliente, string CodigoDeProducto);
        int Insertar(CatalogodeDocumentosDeProductos objCatalogodeDocumentosDeProductos);

        Task<bool> EliminarDocumento(EliminarArchivo objEliminarArchivo);
    }
}