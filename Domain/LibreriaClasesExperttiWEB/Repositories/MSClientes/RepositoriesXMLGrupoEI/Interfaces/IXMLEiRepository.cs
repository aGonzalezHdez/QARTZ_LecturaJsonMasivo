using LibreriaClasesAPIExpertti.Entities.EntitiesXMLGrupoEI;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesXMLGrupoEI.Interfaces
{
    public interface IXMLEiRepository
    {
        string SConexion { get; set; }

        XMLEi ConstruyeXMLWEC_CustomAlert(int IdCustomAlert);
    }
}