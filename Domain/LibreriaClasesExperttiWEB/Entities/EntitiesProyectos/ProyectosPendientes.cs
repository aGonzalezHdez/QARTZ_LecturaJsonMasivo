using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class ProyectosPendientes
    {
        public int idproyecto { get; set; }
        public string Prioridad { get; set; }
        public string Folio { get; set; }
        public string TipodeActividad { get; set; }

        public string Solicitud { get; set; }

        public string Estatus { get; set; }
        public double Porcentaje { get; set; }

        public DateTime FechadeSolicitud { get; set; }

        public DateTime? FechaCompromiso { get; set; }
        public DateTime? FechaFin { get; set; }

        public string Usuario_Solicita { get; set; }

        public string LiderdelProyecto { get; set; }
        public int IdEstatus { get; set; }

    }
}
