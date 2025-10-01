using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioPerParRepository
    {
        string SConexion { get; set; }

        SaaioPerPar? Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART, int PER_IDEN);

        int Insertar(SaaioPerPar objSaaioPerPar);

        int Modificar(SaaioPerPar objSaaioPerPar);

        bool EliminarPorID(string NUM_REFE, int CONS_FACT, int CONS_PART, int PER_IDEN);

        DataTable CargarPermisosDePartida(string NUM_REFE, int CONS_FACT, int Mynum_Part);

    }
}
