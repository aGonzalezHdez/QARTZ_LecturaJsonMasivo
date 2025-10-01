using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesSifty
{
    public class Siftydata
    {
        public string shipper { get; set; }
        public string consignee { get; set; }
        public int packages { get; set; }
        public double weight { get; set; }

        public string description { get; set; }

        public double customs_value { get; set; }

        public string suggestion { get; set; }

    }
}
