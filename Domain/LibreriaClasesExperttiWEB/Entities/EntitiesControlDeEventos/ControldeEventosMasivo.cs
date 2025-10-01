using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class ControldeEventosMasivo
    {
        public int IDCheckPoint { get; set; }
        public int IDUsuario { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Observaciones { get; set; }
        public List<string> Referencias { get; set; }

        public int IdDatosdeEmpresa { get; set; }

    }
}
