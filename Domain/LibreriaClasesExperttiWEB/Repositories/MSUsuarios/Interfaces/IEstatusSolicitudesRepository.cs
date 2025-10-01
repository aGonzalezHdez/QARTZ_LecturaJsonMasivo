using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces
{
    public interface IEstatusSolicitudesRepository
    {
        string SConexion { get; set; }

        List<DropDownListDatos> Cargar();
    }
}
