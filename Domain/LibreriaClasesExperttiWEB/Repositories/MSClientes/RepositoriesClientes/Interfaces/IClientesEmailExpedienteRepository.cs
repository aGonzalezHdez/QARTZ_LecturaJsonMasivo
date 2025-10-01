using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IClientesEmailExpedienteRepository
    {
        string SConexion { get; set; }
        int Insertar(ClientesEmailExpediente objClientesEmailExpediente);
        List<ClientesEmailExpediente> Buscar(int IdCliente);

        int Modificar(ClientesEmailExpediente objClientesEmailExpediente);

        int Eliminar(int IdEmail);
    }
}
