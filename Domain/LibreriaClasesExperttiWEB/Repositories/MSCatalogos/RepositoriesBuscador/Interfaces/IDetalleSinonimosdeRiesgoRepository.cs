using LibreriaClasesAPIExpertti.Entities.EntitiesBuscador;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces
{
    public interface IDetalleSinonimosdeRiesgoRepository
    {
        string SConexion { get; set; }

        DetalleSinonimosdeRiesgo_Bsc Buscar(int idDetalleSR);

        int Insertar(DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc);

        bool Eliminar(int idDetalleSR);

        bool EliminarBuscador(int idBuscar);

        int Modificar(DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc);

        Task<List<SinonimodeRiesgoFotoURI>> CargarUriFotos(int IdSinonimoRiesgo);
    }
}
