using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{

    public class CatalogoDeProductosSubirArchivo
    {
        [Required]
        public int IDCliente { get; set; }

        [Required]
        public string NombreArchivo { get; set; }

        [Required]
        public string ArchivoBase64 { get; set; }

        //Se agrega para registrar en la bitacora quien sube el archivo 01/09/2025 OLE 
  
        public int IdUsuarioAlta { get; set; }
    }
}

