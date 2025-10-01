using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{
    public interface IImagenesGIARepository
    {
        string sConexion { get; set; }

        List<ImagenesGIA> Cargar();
        bool EnviarCorreo(string Archivo);
        bool GetExcelfile();
    }
}