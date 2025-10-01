using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.AdientDXSACI;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces
{
    public interface IApiRestyAdientDXRepository
    {
        string SConexion { get; set; }

        Task<string> Inicio(AdientDXRequets objAdientDXRequets);
    }
}
