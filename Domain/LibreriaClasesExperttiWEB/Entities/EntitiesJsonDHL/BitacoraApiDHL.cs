using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL
{
    internal class BitacoraApiDHL
    {
        public int idBitacora { get; set; }

        public int IdReferencia { get; set; }

        public int Tipo { get; set; }

        public string transferReceiptId { get; set; }

        public bool TieneError { get; set; }

        public string Mensaje { get; set; }

        public string GuiaHouse { get; set; }


    }
}
