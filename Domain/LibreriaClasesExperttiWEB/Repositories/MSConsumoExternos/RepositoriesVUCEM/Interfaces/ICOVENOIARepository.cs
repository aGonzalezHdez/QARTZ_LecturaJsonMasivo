using LibreriaClasesAPIExpertti.Entities.EntitiesVucem;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM.Interfaces
{
    public interface ICOVENOIARepository
    {
        string SConexion { get; set; }

        Task<CoveResponse> GenerarCOVEAsync(CoveRequest coveRequest);
    }
}