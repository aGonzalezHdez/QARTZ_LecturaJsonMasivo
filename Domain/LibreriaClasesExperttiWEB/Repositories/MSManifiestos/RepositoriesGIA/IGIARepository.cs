using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesGIA
{
    public interface IGIARepository
    {
        bool GIADocumentos();
        bool SubirDocumentos(string GuiaHouse, string Archivo);
        ManifiestosErrores SubirporDirectorio();
        ManifiestosErrores ActualizarExiste();
    }
}