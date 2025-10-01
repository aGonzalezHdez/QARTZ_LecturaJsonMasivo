using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeClientesFormalesRepository
    {
        string SConexion { get; set; }

        CatalogoDeClientesFormales Buscar(int MyIdCliente);

        CatalogoDeClientesFormales GetClientesPorClave(string Clave);

        bool GetExisteClientePorRFC(string RFC);

        string Insertar(CatalogoDeClientesFormales objCatalogoDeClientes);

        int Modificar(CatalogoDeClientesFormales objCatalogoDeClientes);

        bool ModificarTipodeFigura(int idCliente, int IDTipodefigura);
      
    }
}