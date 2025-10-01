namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.JsonWEC
{
    public class SDTConsultaWecJson
    {
        public string House { get; set; }
        public string Master { get; set; }
        public string REFI { get; set; }
        public string RegistroEntrada { get; set; }
        public DateTime EntradaAduana { get; set; }
        public string AlmacenArribo { get; set; }
        public string AlmacenNuevo { get; set; }
        public string RevalidadaxAgteExterno { get; set; }
        public string MercanciaAlertada { get; set; }
        public string ClavePedimento { get; set; }
        public string Bultos { get; set; }
        public string Peso { get; set; }
        public string Salida { get; set; }
        public string RevalidaOtroAgenteAduanal { get; set; }
        public string Ubicacion { get; set; }
        public List<Pallets> Pallets { get; set; }
    }

}
