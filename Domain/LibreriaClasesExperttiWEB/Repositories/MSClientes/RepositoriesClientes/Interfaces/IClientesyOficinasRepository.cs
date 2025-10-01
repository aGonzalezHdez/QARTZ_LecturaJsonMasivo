using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IClientesyOficinasRepository
    {
        string SConexion { get; set; }

        List<OficinaporOperacion> CargarficinasporCliente(int IdCliente);
        string EliminarAllOficinas(int IdCliente);
        string Insertar(ClientesyOficinas objClientesyOficinas);
    }
}