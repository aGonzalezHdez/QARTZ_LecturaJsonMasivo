namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    using System.ComponentModel.DataAnnotations;


    public class CatalogodeEjecutivos
    {

        public int IdEjecutivo { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public int IdUsuarioBK { get; set; }

        [Required]
        public int IdOficina { get; set; }

        [Required]
        public int Operacion { get; set; }

        public int IdDepartamento { get; set; }

    }
}