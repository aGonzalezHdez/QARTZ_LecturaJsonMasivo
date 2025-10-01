using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia
{

    public class DocumentosS3
    {
        public string TipodeDocumento { get; set; }
        public string DirectorioVirtual { get; set; }
        public string RutaFisica { get; set; }
        public string Encriptado { get; set; }
        public int IdDocumento { get; set; }

        public string DirectorioVirtualViejo { get; set; }

        public bool S3 { get; set; }

        public string RutaS3 { get; set; }

        public string RutaFisicaAnterior { get; set; }

        public int idTipoDocumento { get; set; }


    }
}
