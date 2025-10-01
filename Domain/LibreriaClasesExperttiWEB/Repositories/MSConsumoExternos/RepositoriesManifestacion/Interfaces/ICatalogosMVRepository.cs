using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces
{
    public interface ICatalogosMVRepository
    {
        string sConexion { get; set; }

        List<DropDownListDatos> Cargar(int idCatalogoMV);
    }
}