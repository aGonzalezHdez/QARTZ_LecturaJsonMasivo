using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class CatalogoSOPProcesos
    {
        public int IDProceso { get; set; }
        public int IDSOP { get; set; }
        public int IDTipoDeProceso { get; set; }
        public int IDDepartamento { get; set; }
        public string Responsables { get; set; }
        public string TiempoDeEntregaAlSProceso { get; set; }
    }
}
