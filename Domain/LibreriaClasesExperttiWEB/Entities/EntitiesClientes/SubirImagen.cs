using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class SubirImagen
    {
        [Required]
        public int IDCliente { get; set; }
        [Required]
        public string CodigoDelProducto { get; set; }
        [Required]
        public int IdTipoDocumento { get; set; }
        [Required]
        public string NombreArchivo { get; set; }
        [Required]
        public string ArchivoBase64 { get; set; }

    }

}