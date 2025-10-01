using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion
{
    public class CatalogosMV
    {
        public int id { get; set; }
        public string cve_casa { get; set; }
        public string ClaveMV { get; set; }
        public string Descripcion { get; set; }
        public int idCatalogoMV { get; set; }
    }
}
