using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface IDetalleApoderadosSiemensRepository
    {
        string SConexion { get; set; }

        int Insertar(DetalleApoderadosSiemens objDetalleApoderadosSiemens);

        DetalleApoderadosSiemens Buscar(int IdDetalle);

        List<DetalleApoderadosSiemens> Cargar();

        bool Modificar(DetalleApoderadosSiemens objDetalleApoderadosSiemens);

        bool Eliminar(int IdDetalle);

    }
}
