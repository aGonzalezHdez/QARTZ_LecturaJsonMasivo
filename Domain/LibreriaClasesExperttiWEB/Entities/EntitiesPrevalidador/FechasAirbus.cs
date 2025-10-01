using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class FechasAirbus
    {
        private int claveFecha;
        public int ClaveFecha
        {
            get { return claveFecha; }
            set { claveFecha = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
    }

}
