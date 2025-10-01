using System.Collections.Generic;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class cmfEncabezado
    {
        public string? File { get; set; }
        public string? datetime { get; set; }
        public string? gateway { get; set; }
        public int? noOfShps { get; set; }

        public List<cmfShipments>? shipments { get; set; }


    }
}
