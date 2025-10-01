using LibreriaClasesAPIExpertti.Entities.EntitiesManifiestoRFC;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesManifiestoRFC
{
    public interface IManifiestoRFCRepository
    {
        string sConexion { get; set; }

        bool descargarFTP();
        List<string> Guardar(ManifiestoRFC obj);
        ManifiestosErrores SubirporDirectorioRFC();
    }
}