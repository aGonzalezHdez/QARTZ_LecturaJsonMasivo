using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeClientesDeDeMensajeriaRepository
    {
        string SConexion { get; set; }

        CatalogoDeClientesDeDeMensajeria Buscar(int idCliente);
        string Insertar(CatalogoDeClientesDeDeMensajeria objCatalogoDeClientesDeDeMensajeria);
        string Modificar(CatalogoDeClientesDeDeMensajeria objCatalogoDeClientesDeDeMensajeria);
        string Mover(string Clave, string RFC, string CURP);
   
    }
}