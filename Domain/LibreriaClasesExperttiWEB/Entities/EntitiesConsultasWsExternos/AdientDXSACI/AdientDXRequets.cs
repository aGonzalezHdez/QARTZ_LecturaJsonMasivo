namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.AdientDXSACI
{
    public class AdientDXRequets
    {
        public string RFC { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;        

        public string NumeroDeReferencia { get; set; } = string.Empty;

        public List<ArchivoBase64> ArchivosBase64 { get; set; } = new();
    }

    public class ArchivoBase64
    {
        public string Nombre { get; set; } = string.Empty;
        public string ContenidoBase64 { get; set; } = string.Empty;
    }
}
