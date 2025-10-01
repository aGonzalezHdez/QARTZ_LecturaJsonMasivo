using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas
{
    public class CatalogodeErroresDetectados
    {
        public int idError { get; set; }

        public string _Error { get; set; } = null!;

        [Required]
        public int? idDepartamento { get; set; }

        [Required]
        public int? idOficina { get; set; }

        public bool? Activo { get; set; }

        public int? IdDepartamentoError { get; set; }

        [Required]
        public int Operacion { get; set; }

        public int Seleccionado { get; set; }

    }
}
