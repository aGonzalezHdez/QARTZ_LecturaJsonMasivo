using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogodeRieles
    {
        public int idRiel { get; set; }
        public int NoRiel { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public string DescripcionWeb { get; set; }
        public bool ActivoWeb { get; set; }

    }
}
