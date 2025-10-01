

namespace LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios
{
    public class SolicitudUsuarios
    {

        public int IdSolicitud { get; set; }
             
        public string Folio { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public int idEstatus { get; set; }

        public int idUsuarioSolicita { get; set; }

        public string Solicita { get; set; }

        public int IDDatosDeEmpresa { get; set; }

        public int IdOficina { get; set; }

        public int IdDepartamento { get; set; }

        public int IDPuesto { get; set; }

        public int Operacion { get; set; }

        public string NSS { get; set; }

        public int NumeroDeEmpleado { get; set; }

        public string Nombre { get; set; }

        public int IDGenero { get; set; }

        public string Email { get; set; }

        public string Iniciales { get; set; }

        public string CURP { get; set; }

        public double MontodeAutorizacion { get; set; }

        public DateTime? IngresoReal { get; set; }

        public DateTime? IngresoANomina { get; set; }

        public string TelParticular { get; set; }

        public string ExtensionTel { get; set; }

        public string Movil { get; set; }

        public string ObservaciondeSolicitud { get; set; }

        public bool Autorizaegreso { get; set; }

        public bool EsUnCliente { get; set; }

        public int idUsuario { get; set; }

        public DateTime? FechaAceptado { get; set; }

        public string UsuarioCASA { get; set; }

    }
}
