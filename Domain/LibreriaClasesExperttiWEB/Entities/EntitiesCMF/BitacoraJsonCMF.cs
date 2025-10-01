namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class BitacoraJsonCMF
    {
        public int idBitacora { get; set; }

        public DateTime IniciaProceso { get; set; }

        public DateTime? TerminaProceso { get; set; }

        public int Guias { get; set; }

        public int Archivos { get; set; }

        public bool Activo { get; set; }

    }
}
