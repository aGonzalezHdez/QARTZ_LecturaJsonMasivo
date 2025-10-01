using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ICtracClientRepository
    {
        string SConexion { get; set; }

        CtracClient Buscar(string MyCve_Imp);
        CtracClient BuscarClientePorReferencia(string NumeroDeReferencia);
    }
}