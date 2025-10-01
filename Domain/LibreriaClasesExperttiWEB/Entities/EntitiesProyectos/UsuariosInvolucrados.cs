using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class UsuarioInvolucrados
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int IdDatoEmpresa { get; set; }
        public string NombreDepartamento { get; set; }
    }
}
