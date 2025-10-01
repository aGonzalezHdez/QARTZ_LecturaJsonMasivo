using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces
{
    public interface ICustomerMasterFileRepository
    {
        string sConexion { get; set; }

        bool AsignaACMF(string GuiaHouse);
        bool Asignar(CustomerMasterFile objCMF);
        cmfCompleto AsignaraAACMF(CatalogoDeUsuarios objUsuario);
        Task<bool> AsignarAAAsync(CustomerMasterFile objCMF, CatalogoDeUsuarios objUsuario);
        cmfCompleto AsignarAPrecaptura();
        bool AsignarGuias(cmfAsignar objAsignar);
        CustomerMasterFile Buscar(string GuiaHouse);
        cmfCompleto BuscarCompleto(int idCMF);
        CustomerMasterFile BuscarId(int idCMF);
        List<CustomerMasterFileAdministracion> CargarAdministracion(DateTime FechaInicial, DateTime FechaFinal, string GuiaHouse);
        List<CustomerMasterFileExistentes> CargarExistentes(DateTime FechaInicial, DateTime FechaFinal, int IdUsuarioAsignado);
        List<CustomerMasterFileArchivos> CargarImagenes(DateTime FechaInicial, DateTime FechaFinal);
        List<CustomerMasterFileNuevos> CargarNuevos(DateTime FechaInicial, DateTime FechaFinal, int IdUsuarioAsignado);
        bool DebeAsignarPrecaptura(int IdCliente);
        int Insertar(CustomerMasterFile lcustomermasterfile);
        int InsertarPieceIdCMF(cmfPieces lcmfPieces);
        Referencias LlenarobjetoReferencia(string GuiaHouse, int idCliente, string GuiaMaster, int idOficina, string AduanaDespacho);
        SaaioPedime LlenarobjSaaioPedime(CustomerMasterFile objCMF);
        int modificarFacturayGuia(CustomerMasterFile objCMF);
        int modificarNuevos(int idCMF, int idDestinatario);
        int modificarPorOrigen(int idCMF, string PaisOrigen, string Descripcion);
        int ModificarPrecaptura(int idCMF);
        int modificarRFC(string guiaHouse, string RFC, string Telefono, string Email);
        int modificarSifty(int idCMF, int sifty);
        int ModificarSimple(CustomerMasterFile lcustomermasterfile);
        List<string> ProcesarLimpieza(List<CustomerMasterFileExistentes> lstCMF);
        bool SinonimosdeRiesgo(string GuiaHouse, string Descripcion, int idPartidasCMF);
    }
}