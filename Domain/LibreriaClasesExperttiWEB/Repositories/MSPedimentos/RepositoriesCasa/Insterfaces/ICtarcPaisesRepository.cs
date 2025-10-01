using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ICtarcPaisesRepository
    {
        string SConexion { get; set; }

        CtarcPaises Buscar(string CVE_PAI);
    }
}