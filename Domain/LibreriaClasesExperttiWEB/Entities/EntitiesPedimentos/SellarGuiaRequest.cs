using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class SellarGuiaRequest
    {
        public int IdReferencia { get; set; }
        public int IdCheckpoint { get; set; }
        public string MisDocumentos { get; set; }
        public LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios Usuario { get; set; }
    }
}
