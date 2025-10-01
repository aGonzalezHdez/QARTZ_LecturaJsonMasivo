using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasaExpertti.Interfaces
{
    public interface ISaaioFacParExperttiRepository
    {
        string SConexion { get; set; }

        SaaioFacpar Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART);          

        List<SaaioFacpar> Cargar(string NUM_REFE, int CONS_FACT);

        int Insertar(SaaioFacpar objSaaio_FacPar);

        bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART);

        int Modificar(SaaioFacpar objSaaioFacPar);
    }
}
