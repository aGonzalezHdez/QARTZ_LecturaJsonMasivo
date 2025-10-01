using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL.Interfaces
{
    public interface IAPIGenerarConAgtAdu
    {
        JsonRepuestaDHL EnvioporGuia(string guia, int idReferencia);
        List<string> EnvioporGuias();
        List<string> EnvioporGuias(List<GuiasPendientes> guias);
        List<string> EnvioporGuiasHilos(int Hilo);
        List<string> EnvioporPedimento(int idBloque);
        string JsonConagtaduGuia(string GuiaHouse, int idReferencia);
        string JsonConagtaduPedimento(List<ConsolAnexos> lstConsolAnexos);
    }
}