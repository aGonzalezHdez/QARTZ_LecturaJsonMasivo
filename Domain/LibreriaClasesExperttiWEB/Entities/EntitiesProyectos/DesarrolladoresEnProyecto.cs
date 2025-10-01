using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class DesarrolladoresEnProyecto
    {
        public int idDesarrolladores { get; set; }
        public int IdDetalle { get; set; }
        public int IdDesarrollador { get; set; }
        public int IdTipo { get; set; }
        public DateTime FechadeAsignacion { get; set; }

        public String Nombre { get; set; }
    }
}
