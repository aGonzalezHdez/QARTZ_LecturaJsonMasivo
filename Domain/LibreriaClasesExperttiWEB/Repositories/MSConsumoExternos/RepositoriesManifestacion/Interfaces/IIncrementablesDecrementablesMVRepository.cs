using LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces
{
    public interface IIncrementablesDecrementablesMVRepository
    {
        string SConexion { get; set; }
        int Insertar(IncrementablesDecrementablesMV objIncrementablesDecrementablesMV);
        bool Modificar(IncrementablesDecrementablesMV objIncrementablesDecrementablesMV);
        bool Eliminar(int idIncrementable);
        //IncrementablesDecrementablesMV Buscar(int idIncrementable);
        List<IncrementablesDecrementablesMV> Cargar();

    }
}
