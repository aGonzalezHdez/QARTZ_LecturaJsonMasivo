namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class cmfShipments
    {
        public string? hAWB { get; set; }
        public string? shpOrgn { get; set; }
        public string? shpDest { get; set; }
        public string? billProdCd { get; set; }
        public string? prodContentCd { get; set; }
        public string? cargodesc { get; set; }
        public string? category { get; set; }
        public string? actWgt { get; set; }
        public string? grossWgt { get; set; }
        public string? pUDate { get; set; }
        public string? sDPieces { get; set; }
        public string? dHLServiceCd { get; set; }
        public string? incoterms { get; set; }
        public string? reasonForExpt { get; set; }
        public string? cstmsVal { get; set; }
        public string? cstmsValCrncyCd { get; set; }

        public cmfConsignor? consignor { get; set; }
        public cmfConsignee? consignee { get; set; }

        public cmfpayer? payer { get; set; }
        public List<cmfLineItems>? lineItems { get; set; }

        public List<cmfPieces>? pieces { get; set; }


    }


}
