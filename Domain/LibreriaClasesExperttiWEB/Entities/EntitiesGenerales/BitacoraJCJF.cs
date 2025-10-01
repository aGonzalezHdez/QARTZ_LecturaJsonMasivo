namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class BitacoraJCJF
    {
        public int idBitacoraJCJF { get; set; }

        public string GuiaHouse { get; set; }

        public DateTime FechaEnvio { get; set; }
        public DateTime FechaModulacion { get; set; }
        

        public bool Respuesta { get; set; }

        public string Mensaje { get; set; }
        public string RFC { get; set; }

        public int Tipo { get; set; }

        public string TipoPrevio { get; set; }

    }
}
