
namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces
{
    public interface IValidacionesCMFRepository
    {
        string SConexion { get; set; }
        string BuscarCuentaFOC(string GuiaHouse);
    }
}