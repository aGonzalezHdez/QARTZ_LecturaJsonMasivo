using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesBoletin
{
    public class BoletinVisto
    {
        public int idVisto { get; set; }
        public int idBoletin { get; set; }
        public int idUsuario { get; set; }
        public DateTime? FechaVisto { get; set; }
    }
}
