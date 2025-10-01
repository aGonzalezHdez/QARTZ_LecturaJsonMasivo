using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class ImagenesGIA
    {
        public int IdCMF { get; set; }

        public string CountryCode { get; set; }
        public string ShipmentId { get; set; }

        public string MAWB { get; set; }

        public string Broker { get; set; }

        public string Registry { get; set; }

        public string Invoice { get;  set; }
    }
}
