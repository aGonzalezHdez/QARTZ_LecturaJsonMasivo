using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesTransCar
{
    public class TransCarError
    {
        public string code { get; set; }

        public List<TransCar_details> details { get; set; }
        public string message { get; set; }
    }
}
