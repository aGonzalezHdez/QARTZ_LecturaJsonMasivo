using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL
{
    internal class JsonPayload
    {
        public string transferReceiptId { get; set; }

        public int providerId { get; set; }

        public DateTime creationDate { get; set; }

        public int numberShipments { get; set; }

        public bool hasErrors { get; set; }

        public string errorDetail { get; set; }

        public string typeInformationReceived { get; set; }

        public string gateway { get; set; }


    }
}
