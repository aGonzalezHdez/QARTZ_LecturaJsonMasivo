namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    using System.ComponentModel.DataAnnotations;

    public class CatalogodeEncargoConferido
    {

        public int IDEncargoConferido { get; set; }

        [Required]
        public int IDCliente { get; set; }

        public string? RFC { get; set; }

        public string? Nombre { get; set; }

        [Required]
        public int IDPatente { get; set; }

        [Required]
        public int Estatus { get; set; }

        [Required]
        public DateTime FechaAceptacion { get; set; }

        [Required]
        public DateTime VigenciaInicio { get; set; }

        public DateTime VigenciaFinal { get; set; }

    }
}