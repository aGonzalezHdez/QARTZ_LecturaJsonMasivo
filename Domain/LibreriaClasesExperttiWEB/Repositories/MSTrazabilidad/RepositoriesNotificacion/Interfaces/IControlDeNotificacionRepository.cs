using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using System.Data;


namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces
{
    public interface IControlDeNotificacionRepository
    {
        string SConexion { get; set; }
        List<DropDownListDatos> CargarVentanasControlNotificacion(int IdUsuario);
        DataTable CargarControlNotificacion(int Ide, int IdUsuario, int IdVentana);
        DataTable CargarControlNotificacionDetalle(int IdFiltro, int IdUsuario, int IdOficina, int IdGrupo, int IdVentana);
    }
}
    