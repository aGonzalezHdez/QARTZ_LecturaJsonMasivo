using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesVucem
{
    public class CoveRequest
    {
        //int idReferencia, int consFact, bool Adenda, int idUsuario, int idUsuarioAutorizaAdenda

        public int idReferencia { get; set; }
        public int consFact { get; set; }

        public bool Adenda { get; set; }

        public int idUsuario { get; set; }

        public int idUsuarioAutorizaAdenda { get; set; }


    }
}
