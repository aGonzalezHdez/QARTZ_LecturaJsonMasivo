namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class RequestJustificarHallazgos
    {
        public RequestCredenciales? Credenciales { get; set; }

        public List<RequestJustificarHallazgosHallazgos>? Hallazgos { get; set; }
    }
}
