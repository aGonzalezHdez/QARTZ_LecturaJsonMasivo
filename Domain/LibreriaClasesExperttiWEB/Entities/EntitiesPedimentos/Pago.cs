using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class Pago
    {
        public string Aduana { get; set; }
        public string Patente { get; set; }
        public int Operacion { get; set; }
        public string Cuenta { get; set; }
        public string Prevalidador { get; set; }
        public int Consecutivo { get; set; }
        public int idOficina { get; set; }
        public int idUsuario { get; set; }
        public List<int> Referencias { get; set; }
    }
}
