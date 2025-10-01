using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos.Interfaces
{
    public interface IAsignacionDeGuiasRepository
    {
        string sConexion { get; set; }

        Task<int[]> Asignar(int IdReferencia, int IdDepartamento, int IdUsuarioAsigna, int IdCheckpointSalida);
        AsignaciondeGuias BuscarUltimoAsignado(int IDReferencia, int IDDepartamento, int IDOficina);
        AsignaciondeGuias BuscarUltimoDepartamento(int IDReferencia);
        DataTable CargarPendientes(int IdUsuario);
        AsignarGuiasRespuesta ReasignarGuia(int IdUsuarioAsigna, int IdUsuarioAsignado, int IdReferencia);
        int ValidaCove(string Referencia);
        int ValidaFastMorning(string Referencia);
    }
}