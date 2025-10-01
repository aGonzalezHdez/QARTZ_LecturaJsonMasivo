using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class Corte
    {
        public int IdCorte { get; set; }
        public string NoCorte { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public int IdSalidaConsol { get; set; }
        public bool CerradoDespachos { get; set; }
        public bool CerradoConsol { get; set; }
        public int IdRelacionBitacora { get; set; } = 0; // Default to 0
        public string Placas { get; set; } = string.Empty; // Default to empty
        public string Tramitador { get; set; } = string.Empty; // Default to empty
        public int IdTramitador { get; set; } = 0; // Default to 0
        public int IdEmpTransportista { get; set; } = 0; // Default to 0
        public int IdDoda { get; set; } = 0; // Default to 0
        public string RelacionBitacora { get; set; }
    }
}
