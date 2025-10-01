using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogodeEjecutivosRepository
    {
        string SConexion { get; set; }

        int ArchivoCargaMasiva(string Archivo, int Oficina, int Operacion, int IdDepartamento);
        List<CatalogodeEjecutivosPorCliente> Cargar(int IdCliente, int IdOficina, int IdUsuario);
        List<DropDownListDatos> CargarDepartamentos(int IdUsuario);
        int Insertar(CatalogodeEjecutivos objCatalogodeEjecutivos);
        int Modificar(CatalogodeEjecutivos objCatalogodeEjecutivos);
        int SubirLayout(CatalogodeEjecutivosSubirLayout objSubirLayout);
        int Eliminar(CatalogodeEjecutivos objCatalogodeEjecutivos);
    }
}