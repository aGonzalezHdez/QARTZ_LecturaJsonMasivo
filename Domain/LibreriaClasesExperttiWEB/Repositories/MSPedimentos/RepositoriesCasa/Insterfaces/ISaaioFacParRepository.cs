using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioFacParRepository
    {
        public string SConexion { get; set; }

        SaaioFacpar Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART);

        List<SaaioFacpar>? Cargar(string NUM_REFE, int CONS_FACT);

        int Insertar(SaaioFacpar objSaaioFacPar);

        int Modificar(SaaioFacpar objSaaioFacPar);

        bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART);
    }
}
