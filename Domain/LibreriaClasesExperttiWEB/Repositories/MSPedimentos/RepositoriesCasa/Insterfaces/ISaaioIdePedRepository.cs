using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioIdePedRepository
    {
        string SConexion { get; set; }

        List<SaaioIdePed>? Cargar(string NUM_REFE);

        SaaioIdePed? Buscar(string NUM_REFE, string CVE_IDEN);

        List<SaaioIdePed>? LlenaDataGridViewSaaioIDePedPorReferencia(string NUM_REFE);

        int Insertar(SaaioIdePed objSaaioIdePed);

        bool Eliminar(string NUM_REFE, int NUM_IDE);

        int Modificar(string NUM_REFE, string CVE_IDEN, string COM_IDEN);
    }
}
