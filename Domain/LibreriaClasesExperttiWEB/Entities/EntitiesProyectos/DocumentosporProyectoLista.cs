using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class DocumentosporProyectoLista
    {
        public string Documento { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaAlta { get; set; }

        public string Nombre { get; set; }

        public string RutaS3 { get; set; }
    }
}
