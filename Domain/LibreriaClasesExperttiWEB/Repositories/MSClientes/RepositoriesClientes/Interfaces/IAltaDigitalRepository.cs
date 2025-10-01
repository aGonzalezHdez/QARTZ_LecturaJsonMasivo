using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IAltaDigitalRepository
    {
        string SConexion { get; set; }

        CatalogoDeClientesFormales GetSolicitudPorRFC(string RFC);
    }
}