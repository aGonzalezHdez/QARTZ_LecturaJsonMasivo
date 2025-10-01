using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogodeEncargoConferidoRepository
    {
        string SConexion { get; set; }

        CatalogodeEncargoConferido Buscar(int IDEncargoConferido);
        List<CatalogodeEncargoConferidoIdCliente> Cargar(int IDCliente);
        bool Eliminar(int IDEncargoConferido);
        bool ExisteEncargo(int IDCliente, int IDPatente);
        int Insertar(CatalogodeEncargoConferido objCatalogodeEncargoConferido);
        int Modificar(CatalogodeEncargoConferido objCatalogodeEncargoConferido);
        List<DropDownListDatos> TipodeDocumento(int Patente);
    }
}