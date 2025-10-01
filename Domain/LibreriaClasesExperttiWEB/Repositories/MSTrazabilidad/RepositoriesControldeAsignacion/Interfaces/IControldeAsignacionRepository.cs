using LibreriaClasesAPIExpertti.Entities.EntitiesControldeAsignacion;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControldeAsignacion.Interfaces
{
    public interface IControldeAsignacionRepository
    {
        string sConexion { get; set; }

        List<ControldeAsignacion> Cargar(ControldeAsignacionRequest objRequest);
        List<ControldeAsignaciónDetalleJas> CargarDetalleJasForwarding(ControldeAsignacionRequest objRequest);
    }
}