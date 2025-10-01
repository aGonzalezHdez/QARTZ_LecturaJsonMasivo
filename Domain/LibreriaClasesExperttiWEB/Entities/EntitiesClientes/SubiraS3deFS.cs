
namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class SubiraS3deFS
    {
        public int IDCliente { get; set; }

        public int IDDocumentosDelCliente { get; set; }

        public string? RutaFisica { get; set; }

        public int IDTiposDeDocumentosDeCliente { get; set; }

        public string? NombreFile { get; set; }

        public bool S3 { get; set; }

        public string RutaS3 { get; set; } = null!;


    }
}
