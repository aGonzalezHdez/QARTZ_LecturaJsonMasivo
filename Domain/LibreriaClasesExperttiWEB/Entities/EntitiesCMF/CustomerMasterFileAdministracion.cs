using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class CustomerMasterFileAdministracion
    {
        public int IdCMF { get; set; }

        public string GuiaHouse { get; set; }

        public double ValorEnDolares { get; set; }

        public int Piezas { get; set; }

        public string Descripcion { get; set; }

        public string Destinatario { get; set; }
   
        public string Cliente { get; set; }
        public string RFC { get; set; }

        public string Telefono { get; set; }

        public string CorreoElectronico { get; set; }

        public string Categoria { get; set; }

        public string Riel { get; set; }

        public string Proveedor { get; set; }

        public DateTime FechaAlta { get; set; }

    }
}
