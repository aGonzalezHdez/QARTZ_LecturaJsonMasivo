using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesManifiestoRFC
{
    public class ManifiestoRFC
    {

        public string idControl { get; set; }

        public string manifestId { get; set; }

        public string gateway { get; set; }

        public string manifestName { get; set; }

        public string origin { get; set; }

        public string flight { get; set; }

        public DateTime flightDateTime { get; set; }

        public List<ManifiestoAWBs> awbs { get; set; }
    }
}
