namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogodeEncargoConferidoIdCliente
    {
        public int IDEncargoConferido { get; set; }

        public int IDCliente { get; set; }

        public int Patente { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaAceptacion { get; set; }

        public DateTime VigenciaInicio { get; set; }

        public DateTime VigenciaFinal { get; set; }
    }

}