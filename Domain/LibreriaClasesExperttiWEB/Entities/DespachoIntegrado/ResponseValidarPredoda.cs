using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class ResponseValidarPredoda
    {
        public List<PredodaDetalle> detalles { get; set; }
        public List<string> errores { get; set; }
    }
}
