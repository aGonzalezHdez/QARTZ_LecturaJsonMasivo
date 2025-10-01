using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IBitacoraDeProveedoresRepository
    {
        string SConexion { get; set; }

        BitacoraDeProveedores Buscar(string CveProv);
        List<BitacoraClientProv> LlenaDataGridViewBitacoraClienteProv(string cveprov);
    }
}