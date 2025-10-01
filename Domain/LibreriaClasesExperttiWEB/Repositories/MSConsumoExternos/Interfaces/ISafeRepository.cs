using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces
{
    public interface ISafeRepository
    {
        string SConexion { get; set; }

        Safe Buscar(int IdOficina);

        List<DropDownListDatos> cargar();
    }
}
