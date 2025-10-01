using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class DocumentosClientes
    {

        public int IDDocumentosDelCliente { get; set; }

        [Required]
        public int IDCliente { get; set; }

        [Required]
        public int IDTiposDeDocumentosDeCliente { get; set; }

        public DateTime? VigenciaDesde { get; set; }

        public DateTime? VigenciaHasta { get; set; }

        [Required]
        public int ClaveDeSubTipoDocumento { get; set; }

        public string NombreFile { get; set; }

        public string? NombreDelCliente { get; set; } 

        public string? Encriptado { get; set; } 

        public DateTime FechaAlta { get; set; }

        public string? RutaVirtual { get; set; }

        public string? TipoDeDocumento { get; set; } 

        public string? DescripcionSubTipo { get; set; }

        public int IDSubTipoDeDocumento { get; set; }

        public string? Ruta { get; set; } 

        public string? RutaVirtualOLD { get; set; }

        public bool S3 { get; set; }

        public string? RutaS3 { get; set; } 

        [Required]
        public int IDCapturo { get; set; }

        public string ArchivoBase64 { get; set; }


    }

}