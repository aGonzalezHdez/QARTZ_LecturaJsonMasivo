namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class PrevalidadorResponse
    {
        public int SinErrores { get; set; }

        public class PrevalidadorError
        {
            public string Pedimento { get; set; }
            public string Referencia { get; set; }
            public string Consecutivo { get; set; }
            public string PartidaCove { get; set; }
            public string PartidaPedi { get; set; }
            public string MensajeError { get; set; }
            public string Sugerencia { get; set; }
            public string TipoDeError { get; set; }
            public string IdError { get; set; }
            public string Justificado { get; set; }
        }
        public string mensaje { get; set; }
        public List<PrevalidadorError> errores { get; set; }
    }
}
