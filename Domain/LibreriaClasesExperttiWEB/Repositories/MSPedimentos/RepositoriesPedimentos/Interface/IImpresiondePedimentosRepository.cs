using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos.Interface
{
    public interface IImpresiondePedimentosRepository
    {
        string SConexion { get; set; }     

        Task<string> GeneraPedimentoPDfCompleto(GenerarPedimento objGenerarPedimento);

        Task<string> GeneraPedimentoPDfSimplificado(GenerarPedimento objGenerarPedimento);
    }
}
