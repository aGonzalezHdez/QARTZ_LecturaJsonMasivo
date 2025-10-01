using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioFacCarRepository
    {
        public string SConexion { get; set; }

        int Insertar(SaaioFacCar lsaaio_faccar);

        DataTable CargarIncrementables(string NUM_REFE, int CONS_FACT);

        bool EliminarIncrementables(string NUM_REFE, int CONS_FACT, int CVE_INCR);
    }
}
