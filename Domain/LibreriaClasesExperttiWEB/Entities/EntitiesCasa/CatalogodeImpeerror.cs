using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCasa
{
    public class CatalogodeImpeerror
    {
        public int idImpeError { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
