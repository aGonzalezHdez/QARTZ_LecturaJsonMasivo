using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IDireccionesDeClientesRepository
    {
        string SConexion { get; set; }

        List<DireccionesDeClientes> BuscarCliente(int IDCliente);

        DireccionesDeClientes BuscarDireccion(int IDDireccion);

        DireccionesDeClientes BuscarDireccionActiva(int IDCliente);

        int Insertar(DireccionesDeClientes objDireccionesDeClientes);

        int Modificar(DireccionesDeClientes objDireccionesDeClientes);
        
    }
}