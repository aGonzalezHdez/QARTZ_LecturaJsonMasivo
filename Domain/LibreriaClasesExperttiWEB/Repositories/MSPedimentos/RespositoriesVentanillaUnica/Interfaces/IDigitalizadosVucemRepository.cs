using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica.Interfaces
{
    public interface IDigitalizadosVucemRepository
    {
        string SConexion { get; set; }

        List<DigitalizadosVucem> CargarMV(string NumerodeReferencia);
    }
}
