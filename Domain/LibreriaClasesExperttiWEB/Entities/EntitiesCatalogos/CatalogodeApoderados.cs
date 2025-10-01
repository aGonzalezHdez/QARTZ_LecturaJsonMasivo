using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogodeApoderados
    {

        public int IdApoderado { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Rfc { get; set; }

        public string? Curp { get; set; } = null!;

        [Required]
        public int IDCliente { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public bool ADefault { get; set; }

        public string? EscrituraPublica { get; set; }

        public DateTime FechaEscritura { get; set; }

        public string? DomicilioFiscal { get; set; }

        public string? CorreoElectronico { get; set; }

        public string? Telefono { get; set; }

    }
}
