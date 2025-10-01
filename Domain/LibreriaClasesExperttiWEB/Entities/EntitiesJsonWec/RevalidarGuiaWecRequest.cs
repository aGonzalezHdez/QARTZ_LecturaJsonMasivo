using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonWec
{
    public class RevalidarGuiaWecRequest
    {
        public string NumeroDeReferencia { get; set; }
        public string Observacion { get; set; }
        public int IdUsuario { get; set; }
        public int IDDatosDeEmpresa { get; set; }
        public int IdDepartamento { get; set; }
        public int IdOficina { get; set; }
    }
}
