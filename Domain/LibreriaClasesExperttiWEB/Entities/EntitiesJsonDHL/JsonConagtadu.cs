using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL
{
    internal class jsonConagtadu
    {
        public int brokerNum { get; set; }

        public int pedimentNum { get; set; }
        public string guia { get; set; }

        public string contenido { get; set; }

        public double valor { get; set; }

        public string userID { get; set; }

        public DateTime dateCapture { get; set; }

        public double prevalidacion { get; set; }

        public double iva { get; set; }

        public double impuestosYDerechos { get; set; }


    }
}
