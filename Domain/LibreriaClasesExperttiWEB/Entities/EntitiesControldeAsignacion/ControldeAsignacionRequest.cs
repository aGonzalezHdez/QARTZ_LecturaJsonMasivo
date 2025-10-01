using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControldeAsignacion
{
    public class ControldeAsignacionRequest
    {
        public int idOficina { get; set; }

        public string? Aduana { get; set; }

        public int? Operacion { get; set; }

        public int idUsuarioFila { get; set; }

        public int Columna { get; set; }

        public int? idDepartamento { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public int idUsuario { get; set; }

    }
}
