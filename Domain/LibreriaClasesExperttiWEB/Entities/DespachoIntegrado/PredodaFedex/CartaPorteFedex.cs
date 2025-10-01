namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaFedex
{
    public class CartaPorteFedex
    {
        public CPFedexbroker broker { get; set; }
        public List<CPFedexbrokerageInfo> brokerageInfo { get; set; }
    }
    public class CPFedexbroker
    {
        public string brokerId { get; set; }
        public string secretcode { get; set; }
    }
    public class CPFedexbrokerageInfo
    {
        public List<cportePackage> packages { get; set; }
    }
    public class cportePackage
    {
        public string awb { get; set; }
        public string pedimento { get; set; }
        public List<CporteCommodities> commodities { get; set; }
    }
    public class CporteCommodities
    {
        public string prodServCd { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string unitCd { get; set; }
        public Double weight { get; set; }
        public Double value { get; set; }
        public string currency { get; set; }
        public string tariffFraction { get; set; }
        public string docType { get; set; }
        public string matterType { get; set; }
        public string matterDesc { get; set; }
        public CPortedg dg { get; set; }
    }
    public class CPortedg
    {
        public string dgCveCd { get; set; }
        public string dgPakTyp { get; set; }
        public string dgPakDesc { get; set; }
    }
}
