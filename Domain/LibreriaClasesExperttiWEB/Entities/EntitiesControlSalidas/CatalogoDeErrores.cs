using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas
{
    public class CatalogoDeErrores
    {
        public int IdError { get; set; }
        public int IdPantalla { get; set; }
        public string Error { get; set; }
        public bool Activo { get; set; }
    }
}
