namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL
{
    public class PredodaWS
    {
        public string PreDodaId { get; set; }
        public string Aduana { get; set; }
        public string Placas { get; set; }
        public string NumeroGafeteUnico { get; set; }
        public string TipoOperacion { get; set; }
        public string Origen { get; set; }
        public List<GuiaDHL> Guias { get; set; }
    }
}
