using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces
{
    public interface ISolicitaSalidaRepository
    {
        string SConexion { get; set; }

        DataTable CargaReporteJCJRPorGuia(int idDatosDeEmpresa, string numerodeReferencia, GuiaHouseRequest guiaHouse);
        DataTable CargaReporteJCJRPorPedimento(int idDatosDeEmpresa, string numerodeReferencia);
        SolicitaSalidaParaJCJF fSolicitarSalida(GuiaHouseRequest guiaHouse);
    }
}