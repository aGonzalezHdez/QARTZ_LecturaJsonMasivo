using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class ProcesarCheckPointRequest
    {
        public string NumeroDeReferencia { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
        public int IdCheckPoint { get; set; }
        public bool SoloAsignar { get; set; } = false;
        public int IdUsuario { get; set; } = 0;
        public int IdDatosDeEmpresa { get; set; } = 0;
        public bool Extraordinario { get; set; } = false;
        public string pMisDocumentos { get; set; } = string.Empty;
    }
}
