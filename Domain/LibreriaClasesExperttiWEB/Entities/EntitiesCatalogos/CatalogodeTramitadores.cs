using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogodeTramitadores
    {
        public int IdTramitador { get; set; }
        public int IdEmpTransportista { get; set; }
        public string Nombre { get; set; }
        public string GafeteUnico { get; set; }
        public bool ACTIVO { get; set; }
    }
}
