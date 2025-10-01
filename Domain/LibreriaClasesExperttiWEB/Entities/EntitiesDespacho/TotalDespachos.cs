using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class TotalDespachos
    {
        public TotalButos totalButos { get; set; }
        public TotalGuias totalGuias { get; set; }
        public List<BultosPedimento> bultosPedimentos { get; set; }
        public List<BultosPedimento> bultosGuias { get; set; }
    }
}
