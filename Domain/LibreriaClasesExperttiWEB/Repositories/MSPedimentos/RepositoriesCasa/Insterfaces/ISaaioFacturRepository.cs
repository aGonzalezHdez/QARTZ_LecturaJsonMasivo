using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioFacturRepository
    {
       public string SConexion { get; set; }

        SaaioFactur? Buscar(string NUM_REFE, int CONS_FACT);

        SaaioFactur? Buscar(string NUM_REFE, string NUM_FACT);

        List<SaaioFactur> Cargar(string NUM_REFE);

        int Modificar(SaaioFactur objSaaioFactur);

        bool BorraFacturaDelCasa(string NUM_REFE, int CONS_FACT);


    }
}
