using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IEstatusEncargoConferidoRepository
    {
        string SConexion { get; set; }

        List<DropDownListDatos> Situaciones();
    }
}