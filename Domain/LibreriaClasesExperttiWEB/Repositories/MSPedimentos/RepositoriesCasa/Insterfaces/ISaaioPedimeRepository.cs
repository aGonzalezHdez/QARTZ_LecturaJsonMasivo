using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioPedimeRepository
    {
        public string SConexion { get; set; }
        SaaioPedime Buscar(string NUM_REFE);
    }
}
