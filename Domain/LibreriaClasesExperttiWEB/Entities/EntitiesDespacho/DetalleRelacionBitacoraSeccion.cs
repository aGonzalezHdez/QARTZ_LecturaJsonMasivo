using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class DetalleRelacionBitacoraSeccionFX
    {
        public string Tipo { get; set; }
        public List<DetalleRelacionBitacoraFX> Items { get; set; }
    }
}
