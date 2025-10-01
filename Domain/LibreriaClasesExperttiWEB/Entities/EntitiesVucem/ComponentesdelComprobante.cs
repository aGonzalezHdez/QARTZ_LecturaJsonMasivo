using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesVucem
{
    public class ComponentesdelComprobante
    {
        public SaaioFactur Factura { get; set; }

        public CtracProved Proveedor { get; set; }

        public wsVentanillaUnica.Emisor Emisor { get; set; }

        public wsVentanillaUnica.Destinatario Destiantario { get; set; }

        public wsVentanillaUnica.Mercancia[] Mercancias { get; set; }

        public int Subdivision { get; set; }

        public int CertificadoOrigen { get; set; }
    }
}
