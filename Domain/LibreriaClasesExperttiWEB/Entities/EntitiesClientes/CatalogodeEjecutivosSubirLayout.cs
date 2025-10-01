namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    using System.ComponentModel.DataAnnotations;


    public class CatalogodeEjecutivosSubirLayout
    {
        [Required]
        public int IdOficina { get; set; }

        [Required]
        public int Operacion { get; set; }

        [Required]
        public int IdDepartamento { get; set; }

        [Required]
        public string NombreArchivo { get; set; }

        [Required]
        public string ArchivoBase64 { get; set; }
    }

}