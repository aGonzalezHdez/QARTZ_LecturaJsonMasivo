using LibreriaClasesAPIExpertti.Entities.EntitiesMV;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioCoveRepository
    {
        string SConexion { get; set; }
        List<SaaioCove> Cargar(string NUM_REFE);

        List<SaaioCoveMV> CargarMV(string NUM_REFE);
    }
}
