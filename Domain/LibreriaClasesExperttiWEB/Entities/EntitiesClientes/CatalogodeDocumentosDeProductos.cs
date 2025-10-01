namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogodeDocumentosDeProductos
    {
        public int IdImagen { get; set; }
        public int IdCliente { get; set; }
        public string CodigoDeProducto { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Archivo { get; set; }
        public bool S3 { get; set; }
        public string RutaS3 { get; set; }
    }

}
