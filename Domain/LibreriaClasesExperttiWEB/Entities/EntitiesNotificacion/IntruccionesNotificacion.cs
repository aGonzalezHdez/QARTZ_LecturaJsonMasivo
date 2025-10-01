namespace LibreriaClasesAPIExpertti.Entities.EntitiesNotificacion
{
    public class IntruccionesNotificacion
    {
        public int IdInstrucciones { get; set; }
        public int IdReferencia { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string CveProv { get; set; }
        public string Regimen { get; set; }
        public DateTime FechaAlta { get; set; }
        public string ReferenciaCliente { get; set; }
        public string Ejecutivo { get; set; }

    }
}
