using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class ControldeEventosDoda
    {

        public int IDCheckPoint { get; set; }
        public int IDUsuario { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Observaciones { get; set; }
        public int IdDatosdeEmpresa { get; set; }

        public int idDODA { get; set; }

    }
}
