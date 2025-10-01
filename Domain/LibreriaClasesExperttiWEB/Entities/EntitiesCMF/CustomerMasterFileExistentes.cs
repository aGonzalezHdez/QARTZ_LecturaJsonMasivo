using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class CustomerMasterFileExistentes
    {

        public int IdCMF { get; set; }

        public string GuiaHouse { get; set; }

        public string Destinatario { get; set; }

        public string Cliente { get; set; }

        public string Categoria { get; set; }

        public bool ClienteCorrecto { get; set; }


    }
}