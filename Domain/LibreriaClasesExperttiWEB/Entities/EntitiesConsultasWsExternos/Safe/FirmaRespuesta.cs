namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class FirmaRespuesta
    {
        public bool firmado { get; set; }

        public string? mensaje { get; set; }

        public List<FirmaHallazgosRespuesta>? hallazgos { get; set; }

        public string? archivo { get; set; }
    }
}
