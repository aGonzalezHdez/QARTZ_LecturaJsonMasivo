namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogoDeUsuarios
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string Psw { get; set; } = null!;

        public int IdOficina { get; set; }

        public string Email { get; set; } = null!;

        public bool Autorizaegreso { get; set; }

        public double MontodeAutorizacion { get; set; }

        public int IdDepartamento { get; set; }

        public int IdModulo { get; set; }

        public bool UsuarioActivo { get; set; }

        public int CapturoUsuario { get; set; }

        public DateTime FechaDeCaptura { get; set; }

        public bool EsUnCliente { get; set; }

        public string UsuarioCASA { get; set; } = null!;

        public bool SolicitarCambioDePassword { get; set; }

        public bool MostrarTodosLosEventos { get; set; }

        public string Iniciales { get; set; } = null!;

        public string CURP { get; set; } = null!;

        public int IDGenero { get; set; }

        public int IDPuesto { get; set; }

        public DateTime IngresoReal { get; set; }

        public DateTime IngresoANomina { get; set; }

        public DateTime Baja { get; set; }

        public int NumeroDeEmpleado { get; set; }

        public bool PoolPendientes { get; set; }

        public int IDDatosDeEmpresa { get; set; }

        public string ExtensionTel { get; set; } = null!;

        public int IdEstacionDefault { get; set; }

        public int Operacion { get; set; }

        public bool TomarTelPart { get; set; }

        public string TelParticular { get; set; } = null!;

        public string UsuarioWindows { get; set; } = null!;

        public bool UsuarioAutomatico { get; set; }

        public bool PermitirVariosExpertit { get; set; }

        public int UsuarioBaja { get; set; }

        public DateTime FechaUsuarioBaja { get; set; }

        public string MotivoBaja { get; set; } = null!;

        public CatalogoDeOficinas Oficina { get; set; } = null!;

    }
}
