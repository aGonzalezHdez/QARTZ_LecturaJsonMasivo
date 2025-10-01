using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICuentasPECERepository
    {
        string SConexion { get; set; }

        int BorrarRelacionClientePECE(string CveCta);
        CuentasPECE BuscarDetalleCveCtaPece(string CveCta);
        List<Saaic_Ctaban> BuscarPorCuenta(string NumerodeCuenta);
        List<Saaic_Ctaban> BuscarPorCuentaDuplicada(string NumerodeCuenta, string Patente, string Aduana);
        int CambiaRPECA(CuentasPECE objCuentasPECE);
        List<CargarRelClientesCuentasPECA> CargarRelClientesCuentasPECA(string CveCta);
        List<CuentasdelCliente> CuentasdelCliente(int IdCliente);
        int EliminarRegistros(string Clave, string NUM_CTA);
        string Insertar(CuentasPECE objCuentasPECE);
        string InsertarCuenta(CuentasPECE objCuentasPECE);
        int InsertarRelacionClientePECE(string CVE_CTA, string CVE_IMP, int CTAPROPIA, int IDOFICINA, int IDDATOSDEEMPRESA, int IdUsuario);
        string ModificarCuenta(CuentasPECE objCuentasPECE);
        string ValidacionesComponentes(CuentasPECE objCuentasPECE);
    }
}