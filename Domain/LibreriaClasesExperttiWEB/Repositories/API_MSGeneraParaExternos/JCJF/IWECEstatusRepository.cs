using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;

namespace LibreriaClasesAPIExpertti.Repositories.API_MSGeneraParaExternos.JCJF
{
    public interface IWECEstatusRepository
    {
        string sConexion { get; set; }

        int Insertar(WECEstatus objWECEstatus);
    }
}