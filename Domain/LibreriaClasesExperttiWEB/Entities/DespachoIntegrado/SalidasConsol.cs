using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class SalidasConsol
    {
        public int IDSalidaConsol { get; set; }
        public string NumeroSalidaConsol { get; set; }
        public DateTime Fecha { get; set; }
        public int IDUsuario { get; set; }
        public bool Estatus { get; set; }
        public string NoCorte { get; set; }
        public int IdCorte { get; set; }
        public int IDRegion { get; set; }
        public string Region { get; set; }
        public string IdOficina { get; set; }
        public int IdMasterConsol { get; set; }
        public int IdRelacionBitacora { get; set; }
        public int IdTramitador { get; set; }
        public int IdEmpTransportista { get; set; }
        public string Placas { get; set; }
        public int IdPreDoda { get; set; }
        public string RFC { get; set; }
    }
}
