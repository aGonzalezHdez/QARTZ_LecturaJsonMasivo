using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControldeAsignacion
{
    public class ControldeAsignacion
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public int Inventario { get; set; }

        public int DiasAnteriores { get; set; }

        public int HoyMismo { get; set; }

        public int PendientesHoy { get; set; }

        public int NotificadasHoy { get; set; }

        public int AltaPrioridad { get; set; }


    }
}                  
