using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios
{
    public class UsuariosDisponibles
    {
        public int IdUsuarioDisponible { get; set; }

        public int IdUsuario { get; set; }

        public int IdDepartamento { get; set; }

        public int IdOficina { get; set; }

        public DateTime Fecha { get; set; }
    }
}
