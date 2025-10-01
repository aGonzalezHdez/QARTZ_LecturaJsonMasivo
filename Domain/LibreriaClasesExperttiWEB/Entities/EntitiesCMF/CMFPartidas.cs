using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class CMFPartidas: cmfLineItems
    {
        public int idPartidasCMF { get; set; }

        public int idCMF { get; set; }

        public int idCategoriaSR { get; set; }
    }
}
