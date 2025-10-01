namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class ObtenerHallazgosHallasgos
    {
        public string? pedimento { get; set; }

        public string? archivo { get; set; }

        public string? descripcion { get; set; }

        public string? errorLiberacion { get; set; }

        public int idHallazgo { get; set; }

        public int idError { get; set; }

        public int id { get; set; }

        public int severidad { get; set; }

        public bool seleccionado { get; set; }
    }
}
