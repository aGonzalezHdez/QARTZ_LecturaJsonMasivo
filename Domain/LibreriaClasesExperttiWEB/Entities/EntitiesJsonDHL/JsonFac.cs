using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL
{
    internal class JsonFac
    {
        public string reference { get; set; }

        public string pediment_num { get; set; }

        public DateTime fecha_entrada { get; set; }

        public int bultos { get; set; }

        public double peso { get; set; }

        public string guia { get; set; }

        public double valor_mercan { get; set; }

        public double valor_factura { get; set; }

        public double valor_aduana { get; set; }

        public string descripcion { get; set; }

        public double tipo_cambio { get; set; }

        public string tipo_pediment { get; set; }

        public string clave_pediment { get; set; }

        public double valor_increm { get; set; }

        public DateTime fecha_pago_ped { get; set; }

        public double impuestos { get; set; }

        public double servicios { get; set; }

        public double total_factura { get; set; }

        public string fkrfc { get; set; }

        public string razon_social { get; set; }

        public string direccion { get; set; }

        public string no_ext { get; set; }

        public string no_int { get; set; }

        public string colonia { get; set;}

        public string cp { get; set; }

        public string work_num { get; set; }

        public string stat_trx { get; set; }

        public DateTime date_trx { get; set; }

        public int numctl_net { get; set; }

        public string calle { get; set; }

        public string estado { get; set; }

        public string municipio { get; set; }




    }
}
