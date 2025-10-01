using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces
{
    public interface IPermisosyReportesporUsuarioRepository
    {
        string SConexion { get; set; }

        int Insertar(int IDUsuario, int IDReporte);

        int Eliminar(int IDPermisosyReportes, int IDUsuario);

        public List<PermisosYReportesporUsuario> Cargar(int IDUsuario);
    }
}
