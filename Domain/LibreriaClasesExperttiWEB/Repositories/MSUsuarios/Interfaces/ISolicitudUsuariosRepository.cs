using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces
{
    public interface ISolicitudUsuariosRepository
    {
        string SConexion { get; set; }

        string Insertar(SolicitudUsuarios objSolicitudUsuarios);

        int Modificar(SolicitudUsuarios objSolicitudUsuarios);

        SolicitudUsuarios Buscar(string Folio); 

        List<SolicitudUsuarios> BuscarEstatus(int idEstatus);

        List<SolicitudUsuarios> PendientesporUsuario(int idUsuarioSolicita);

        List<SolicitudUsuariosCoincidencias> Coincidencias(int IdSolicitud);
    }
}
