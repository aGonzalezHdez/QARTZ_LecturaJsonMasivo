using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesBuscador
{
    public class Imagenes_Bsc
    {
        public int idImagen { get; set; }

        public int idBuscar { get; set; }

        public string RutaS3 { get; set; }

        public int Consecutivo { get; set; }
      
        public string? NombreArchivo { get; set; }
       
        public string? ArchivoBase64 { get; set; }
    }
}
