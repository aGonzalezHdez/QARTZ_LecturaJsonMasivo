using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using NPOI.SS.UserModel;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICatalogoDeProductosPorClienteRepository
    {
        string SConexion { get; set; }

        CatalogoDeProductosPorCliente Buscar(int MyIdCliente, string MyCodigoDeProducto);

        List<CatalogoDeProductosPorClientePaginado> Cargar(int IdCliente, int PageNumber);

        Task<string> CargarArchivoDeExcel(CatalogoDeProductosSubirArchivo objCatalogoDeProductosMasivo);
        
        Task<string> CargarArchivoDeExcelVariosClientes(CatalogoDeProductosSubirArchivo objCatalogoDeProductosMasivo);

        List<HistoricodeFracciones> CargarHistorico(string Clave, int NoPagina);
  
        int Insertar(CatalogoDeProductosPorCliente objCatalogoDeProductosPorCliente);

        int Modificar(CatalogoDeProductosPorCliente objCatalogoDeProductosPorCliente);

        Task<string> SubirImagen(SubirImagen objSubirImagen);

        List<DropDownListDatos> TipodeDocumento();
        
        Task<string> RegistraBitacoraMasivos(int IdCliente, string NombreArchivo, int IdUsuario);

    }
}