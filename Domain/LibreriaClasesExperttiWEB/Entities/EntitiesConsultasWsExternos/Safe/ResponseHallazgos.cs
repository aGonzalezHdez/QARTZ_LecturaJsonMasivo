namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class ResponseHallazgos
    {
        public bool success { get; set; }

        public string? message { get; set; }

        public List<ResponseHallazgosValidarHallazgos>? Hallazgos { get; set; }

        public List<ResponseHallazgosValidarJustificacionesTL>? JustificacionesTL { get; set; }

        public List<string>? Errores { get; set; }

        public string? ErroresLayout { get; set; }

        public string? HallazgosLayout { get; set; }

        public bool bError { get; set; }

        public string? Error { get; set; }
    }
}
