using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesRFC
{
    public class RegistroRFCDHL
    {
        public string TaxIDImpo { get; set; }
        public int IdRiel { get; set; }
        public string GuiaHouse { get; set; }
        public string DestinatarioEmail { get; set; }
        public string DatosContacto { get; set; }
        public string Nombre { get; set; }
        public int IdUsuario { get; set; }
        public string CURP { get; set; }
    }
}
