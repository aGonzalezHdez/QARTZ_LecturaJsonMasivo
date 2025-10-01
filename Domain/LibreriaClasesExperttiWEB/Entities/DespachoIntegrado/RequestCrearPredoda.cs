using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class RequestCrearPredoda
    {
        public string Aduana { get; set; }
        public string Patente { get; set; }
        public int Operacion { get; set; }
        public string Origen { get; set; }
        public string Placas { get; set; }
        public int idRelacionBitacora { get; set; }
        public int idRelaciondeEnvio { get; set; }
        public int idSalidasConsol { get; set; }
    }
}
