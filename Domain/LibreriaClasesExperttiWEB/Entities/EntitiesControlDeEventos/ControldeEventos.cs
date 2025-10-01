namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class ControldeEventos
    {
        public ControldeEventos(int pIdCheckPont, int pIdReferencia, int pIdUsuario, DateTime pFechaEvento)
        {
            IDCheckPoint = pIdCheckPont;
            IDReferencia = pIdReferencia;
            IDUsuario = pIdUsuario;
            FechaEvento = pFechaEvento;


        }

        public ControldeEventos()
        {
        }
        public int IDEvento { get; set; }

        public int IDCheckPoint { get; set; }
        public int IDReferencia { get; set; }
        public int IDUsuario { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Observaciones { get; set; }
        public int IdDepartamento { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string ObservacionCompleta { get; set; }
        public string Color { get; set; }
        public string NombreUsuario { get; set; }
    }
}
