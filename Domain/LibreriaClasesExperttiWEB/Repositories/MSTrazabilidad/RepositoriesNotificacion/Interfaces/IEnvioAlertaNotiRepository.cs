using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces
{
    public interface IEnvioAlertaNotiRepository
    {
        string sConexion { get; set; }

        List<DropDownListDatos> CargarRequisitosAlertasNoti();
        List<DropDownListDatos> CargarSubRequisitoAlertasNoti(int IdRequisitoAlerta);
    }
}