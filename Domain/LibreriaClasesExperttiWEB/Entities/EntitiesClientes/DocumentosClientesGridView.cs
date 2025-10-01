namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class DocumentosClientesGridView
    {

        public string TipoDeDocumento { get; set; }

        public int IDDocumentosDelCliente { get; set; }

        public string RutaVirtual { get; set; }

        public string Encriptado { get; set; }

        public string RutaFisica { get; set; }

        public int IDTiposDeDocumentosDeCliente { get; set; }

        public string DescripcionStatus { get; set; }

        public string Observacion { get; set; } = null!;

        public string NombreFile { get; set; }

        public bool S3 { get; set; }

        public string RutaS3 { get; set; } = null!;


    }

}