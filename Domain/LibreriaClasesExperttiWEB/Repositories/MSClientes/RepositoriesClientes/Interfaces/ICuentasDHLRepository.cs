using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICuentasDHLRepository
    {
        string SConexion { get; set; }

        List<CuentasDHL> BuscarSiExisteCuentaDHL(int IDCliente, string NumerodeCuenta);
        Task<List<CuentasDHL>> Cargar(int IdCliente);
        Task<bool> Eliminar(int idCuenta);
        Task<int> Insertar(CuentasDHL objCatalogoDeCuentasDHL);
        string ValidaSiExisteCuentaDHL(CuentasDHL objCatalogoDeCuentasDHL);
    }
}