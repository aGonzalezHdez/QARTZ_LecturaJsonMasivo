using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class EstatusDesarrollo
    {
        public int IdEstatus{ get; set; }
        public string Estatus { get; set; }
        public Boolean FechaRequerida { get; set; }
        public int IdDepartamento { get; set; }
    }
}
