using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class DocumentosporProyectosExt
    {
        public int IDProyecto { get; set; }
        public int IdUsuario { get; set; }

        public int idTipo { get; set; }

        public string NombreArch { get; set; }

        public string Extension { get; set; }

        public string ArchivoBase64 { get; set; }


        
    }
}
