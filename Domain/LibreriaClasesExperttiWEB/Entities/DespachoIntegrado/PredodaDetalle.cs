namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class PredodaDetalle
    {
        public int idPreDodaDetalle { get; set; }
        public int idPreDoda { get; set; }
        public int IdReferencia { get; set; }
        public string Referencia { get; set; }
        public string NumeroDeCOVE { get; set; }
        public string RutaS3 { get; set; }
        public string Pedimento { get; set; }
        public int Remesa { get; set; }
        public string RFC { get; set; }
    }
}
