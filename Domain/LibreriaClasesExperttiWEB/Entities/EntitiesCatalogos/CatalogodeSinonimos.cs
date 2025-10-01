using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogodeSinonimos
    {

        public int IDSinonimos { get; set; }

        [Required]
        public string Sinonimo { get; set; } = null!;

        [Required]
        public int IDCategoria { get; set; }

        [Required]
        public string NombreDelCliente { get; set; } = null!;

        [Required]
        public string RFC { get; set; } = null!;

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IDUsuario { get; set; }

        public DateTime FechaDeAlta { get; set; }

    }
}
