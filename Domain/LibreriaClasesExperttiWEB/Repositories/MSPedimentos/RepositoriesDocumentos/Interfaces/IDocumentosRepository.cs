using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesDocumentos.Interfaces
{
    public interface IDocumentosRepository
    {
        string SConexion { get; set; }

        Errores SubirDocumento();
    }
}
