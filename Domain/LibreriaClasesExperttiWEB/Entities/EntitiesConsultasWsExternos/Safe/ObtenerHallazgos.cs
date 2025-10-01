namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class ObtenerHallazgos
    {
        public string? mensaje { get; set; }

        public bool correcto { get; set; }

        public List<ObtenerHallazgosHallasgos>? hallazgos { get; set; }
    }
}
