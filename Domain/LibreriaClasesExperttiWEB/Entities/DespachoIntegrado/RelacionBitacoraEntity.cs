using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class RelacionBitacoraEntity
    {
        public int IdRelacionBitacora { get; set; }
        public int IdTiposervicio { get; set; }
        public string RelacionBitacora { get; set; }
        public int IdEstacion { get; set; }
        public int IdUsuario { get; set; }
        public int Consecutivo { get; set; }
        public int Proceso { get; set; }
        public DateTime FechaBitacora { get; set; }
        public int Estatus { get; set; }
        public DateTime FechaCierre { get; set; }
        public int Ruta { get; set; }
        public string Placas { get; set; }
        public string Gafete { get; set; }
        public int TipoBitacora { get; set; }
        public int IdTramitador { get; set; }
        public string IdEmpTransportista { get; set; }
        public string Tramitador { get; set; }
        public int IdDoda { get; set; }
        public int IDDatosDeEmpresa { get; set; }
        public string RFC { get; set; }
    }
}
