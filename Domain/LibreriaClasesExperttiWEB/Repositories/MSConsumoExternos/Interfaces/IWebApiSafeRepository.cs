using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces
{
    public interface IWebApiSafeRepository
    {
        string SConexion { get; set; }

        RespuestaJuliano GenerarJulianoSafe(JulianoSiemens objJulianoSiemens);

        Task<ResponseHallazgos> ObtenerHallazgos(RutaJuliano objRutaJuliano);

        Task<ResponseHallazgos> ObtenerHallazgosJuliano(JulianoSiemens objJulianoSiemens);        

        Task<ResponseHallazgos> JustificarHallazgos(List<RequestJustificarHallazgosHallazgos> lstJustificarHallazgos, int IdReferencia, int IdUsuario);

        List<DropDownListDatos> CargarTiposdeFiltro();

        List<DropDownListDatos> CargarJustificaciones();

    }
}
