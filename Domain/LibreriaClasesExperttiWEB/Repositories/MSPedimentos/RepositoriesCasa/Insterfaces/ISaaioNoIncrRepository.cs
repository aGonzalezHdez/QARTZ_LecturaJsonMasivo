using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioNoIncrRepository
    {
        string SConexion { get; set; }
        DataTable Cargar(string NUM_REFE);
    }
}
