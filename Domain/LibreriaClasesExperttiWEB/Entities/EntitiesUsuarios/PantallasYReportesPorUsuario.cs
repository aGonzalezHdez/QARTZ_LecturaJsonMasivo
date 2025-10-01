namespace LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios
{
    public class PantallasYReportesPorUsuario
    {
        public int IdUsuario { get; set; }

        public int IdPantalla { get; set; } 

        public string NivelDeAcceso { get; set; }

        public string Modulo { get; set; }

        public string Descripcion { get; set; }

    }
}
