using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControldeAsignacion
{
    public class ControldeAsignaciónDetalleJas
    {
        public string TOperacion { get; set; }
        public string AduanaDespacho { get; set;}

        public string Aduana { get; set; }

        public string NumeroDeReferencia {  get; set;}

        public string Patente { get; set; }
        public DateTime FechaAsignacion { get; set; }

        public string Cliente { get; set; }

        public string Pedimento { get; set; }

        public string GuiasHouse { get; set; }

        public string UltimaLlamada { get; set; }

        public string UltimoCheck { get; set; }

        public int Prioridad { get; set; }


    }
}
