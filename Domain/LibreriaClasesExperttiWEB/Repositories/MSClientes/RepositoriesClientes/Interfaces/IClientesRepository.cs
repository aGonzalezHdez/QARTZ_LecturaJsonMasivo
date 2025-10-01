using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IClientesRepository
    {
        string SConexion { get; set; }

        Clientes Buscar(int IdCliente);
        List<ClientesProveedores> CargarProveedores(int IDCliente, int Operacion);
        DataTable CargarProveedores(int IDCLIENTE, int Operacion, string myConnectionString);
    }
}