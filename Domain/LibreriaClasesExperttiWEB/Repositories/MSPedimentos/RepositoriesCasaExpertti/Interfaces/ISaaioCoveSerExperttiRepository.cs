using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasaExpertti.Interfaces
{
    public interface ISaaioCoveSerExperttiRepository
    {
        string SConexion { get; set; }

        public DataTable Cargar(string NUM_REFE);

        int Insertar(SaaioCoveSer lsaaio_coveser);

        int Modificar(SaaioCoveSer lsaaio_coveser);

        bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART, int CONS_SERI);

        SaaioCoveSer? Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART, int CONS_SERI);
    }
}
