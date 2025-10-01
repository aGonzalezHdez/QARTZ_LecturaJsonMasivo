using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogodeSellosDigitalesRepository
    {
        string SConexion { get; set; }

        CatalogodeSellosDigitales Buscar(string UsuarioWebService);
        CatalogodeSellosDigitales BuscarMensajeria();
        int Insertar(CatalogodeSellosDigitales objCatalogodeSellosDigitales);
        int Modificar(CatalogodeSellosDigitales objCatalogodeSellosDigitales);
        bool ProbarCredenciales(string Usuario, string PasswordWS);
        void ValidaPasswordKey(string ArchivoBase64, string RFC, string Password);
        ValidaCertificado ValidarCer(string NormbreArchivo, string sUsuarioWS, bool rdoClientes);
        void ValidarObjeto(CatalogodeSellosDigitales objCatalogodeSellosDigitales, bool esInsertar);
    }
}