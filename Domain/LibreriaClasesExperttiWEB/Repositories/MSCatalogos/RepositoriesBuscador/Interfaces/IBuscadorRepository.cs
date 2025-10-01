using LibreriaClasesAPIExpertti.Entities.EntitiesBuscador;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces
{
    public interface IBuscadorRepository
    {
        string SConexion { get; set; }

        List<SinonimosdeRiesgoTodos> SinonimosdeRiesgoTodos(string Descripcion);

        Task<int> InsertarBuscador(BuscadorBuscador objBuscador);

        int Insertar(BuscadorBuscador objBuscador);
        BuscadorBuscador BuscarBuscador(int idBuscar);

        BuscadorBuscador Buscar(int idBuscar);

        List<BuscadorBuscador> LoadBuscador(string PalabraClave);

        Task<int> ModificarBuscador(BuscadorBuscador objBuscador);

        int Modificar(BuscadorBuscador objBuscador);
        Task<bool> EliminarBuscador(int idBuscar);
    }
}
