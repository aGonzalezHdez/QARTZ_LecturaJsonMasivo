using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class DocumentosporProyecto
    {
        public int idDocumento { get; set; }
        public int IdProyecto { get; set; }
        public int consecutivo { get; set; }
        public string RutaS3 { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuario { get; set; }
        public int idTipo { get; set; }
    }
}
