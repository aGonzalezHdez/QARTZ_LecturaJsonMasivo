using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces
{
    public interface ISaaioCoveSerRepository
    {
        public string SConexion { get; set; }

        SaaioCoveSer? Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART, int CONS_SERI);

        List<SaaioCoveSer>? CargarSeries(string NUM_REFE, int CONS_FACT, int CONS_PART);

        DataTable Cargar(string NUM_REFE);

        int Insertar(SaaioCoveSer objSaaioCoveSer);

        int Modificar(SaaioCoveSer objSaaioCoveSer);

        bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART, int CONS_SERI);
    }
}
