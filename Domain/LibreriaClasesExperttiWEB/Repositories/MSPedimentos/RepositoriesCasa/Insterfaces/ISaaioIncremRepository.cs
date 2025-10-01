using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioIncremRepository
    {
        string SConexion { get; set; }
        DataTable Cargar(string NUM_REFE);
    }
}
