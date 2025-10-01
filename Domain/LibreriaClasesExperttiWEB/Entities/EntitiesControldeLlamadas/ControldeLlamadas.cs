namespace LibreriaClasesAPIExpertti.Entities.EntitiesControldeLlamadas
{
    public class ControlDeLlamadas
    {
        public int Llamada { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string Notificador { get; set; }
        public bool LlamadaEfectiva { get; set; }
        public string Telefono { get; set; }
        public bool NoSujetaNotifica { get; set; }
        public int IdMensaje { get; set; }
        public int IdUsuario { get; set; }
        public string IdReferencia { get; set; }
    }

}
