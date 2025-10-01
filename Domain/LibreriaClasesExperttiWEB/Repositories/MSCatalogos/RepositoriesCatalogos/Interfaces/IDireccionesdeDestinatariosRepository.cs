using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface IDireccionesdeDestinatariosRepository
    {
        string sConexion { get; set; }

        DireccionesdeDestinatarios Buscar(int IDDireccion);
        List<DireccionesdeDestinatarios> Cargar(int IdDestinatario);
        List<DireccionesdeDestinatarios> Coincidencia(string CodigoPostal, string Calle, string NumeroExt);
        int Insertar(DireccionesdeDestinatarios objDireccionesdeDestinatarios);
        int Modificar(DireccionesdeDestinatarios objDireccionesdeDestinatarios);
    }
}