namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class RequestArchivo
    {
        public string? rfc { get; set; }

        public string? fileName { get; set; }

        public string? JulianoBase64 { get; set; }

        public bool envioNotificacionSuite { get; set; }
    }
}
