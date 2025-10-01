using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class SeguimientoDesarrollo
    {
        public int IdSeguimiento { get; set; }
        public int IdDetalle { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public DateTime Fecha { get; set; }

    }
}
