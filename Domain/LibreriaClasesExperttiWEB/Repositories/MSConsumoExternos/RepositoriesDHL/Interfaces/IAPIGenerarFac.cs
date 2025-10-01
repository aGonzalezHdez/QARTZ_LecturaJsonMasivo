using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL.Interfaces
{
    public interface IAPIGenerarFac
    {
        List<string> EnviarPendientes();
        JsonRepuestaDHL EnvioporReferencia(int IdReferencia);
        string JsonFac(int vidReferencia);
    }
}