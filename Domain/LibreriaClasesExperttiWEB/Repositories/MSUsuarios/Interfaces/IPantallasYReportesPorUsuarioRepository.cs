using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces
{
    public interface IPantallasYReportesPorUsuarioRepository
    {
        string SConexion { get; set; }

        List<PantallasYReportesPorUsuario> Cargar(int IdUsuario);

        void Insertar(Pantallas objPantallas);

        void Eliminar(Pantallas objPantallas);
    }
}
