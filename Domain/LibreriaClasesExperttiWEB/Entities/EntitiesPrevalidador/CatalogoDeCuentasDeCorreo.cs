using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class CatalogoDeCuentasDeCorreo
    {
        public int IDMail { get; set; }
        public string Nombre { get; set; }
        public string gEMail { get; set; }
        public string gPassMail { get; set; }
        public string gHost { get; set; }
        public int gPuertoMail { get; set; }
        public bool EnableSsl { get; set; }
        public bool CuentaActiva { get; set; }
        public bool Relay { get; set; }
    }
}
