using LibreriaClasesAPIExpertti.Entities.EntitiesBuscador;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces
{
    public interface IImagenes_BscRepository
    {
        string SConexion { get; set; }

        //int Insertar(Imagenes_Bsc objIimagenes);

        Task<int> InsertarImagenes(Imagenes_Bsc imagenes);
        Imagenes_Bsc Buscar(int idImagen);

        bool Eliminar(int idImagen);

        Task<bool> EliminarImagenes(int idBuscar);

        int Modificar(Imagenes_Bsc objIimagenes);
    }
}
