namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class RequestJustificarHallazgosHallazgos
    {
        public string? PedimentoId { get; set; }

        public string? RFC { get; set; }

        public int CriterioId { get; set; }

        public string? Criterio { get; set; }

        public string? MensajeCriterio { get; set; }

        public string? Contrasena { get; set; }

        public string? Justificacion { get; set; }

        public int ErrorId { get; set; }

        public string? RespuestaCriterios { get; set; }

        public int Severidad { get; set; }

        public string? Extension { get; set; }

        public string? Base64 { get; set; }

        public string? Nombre { get; set; }
    }
}
