namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class FacturasCove
    {
        public int IDFacturaCOVE { get; set; }
        public int IDReferencia { get; set; }
        public int CONS_FACT { get; set; }
        public string CadenaOriginal { get; set; }
        public string NumeroDeCOVE { get; set; }
        public string FirmaDigital { get; set; }
        public bool COVE { get; set; }
        public int NumeroDeOperacion { get; set; }
        public string FirmaDigitalOperacion { get; set; }
        public string CadenaOriginalOperacion { get; set; }
        public DateTime FechaDeEnvio { get; set; }
        public DateTime FechaDeRecibido { get; set; }
        public bool EnviadoSAT { get; set; }
        public string numeroAdenda { get; set; }
        public int idDocumento { get; set; }
        public int idDocumentoXML { get; set; }
        public int idDocumentoCove { get; set; }
        public int idDocumentoAcuseXML { get; set; }
        public string validacion_agencia { get; set; }
        public string FolioDVC { get; set; }
    }
}
