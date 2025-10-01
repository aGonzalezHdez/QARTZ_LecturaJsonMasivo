using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesTransCar
{
    public class TransCarPredoda
    {
        public string Code { get; set; }
        public string Pedimento { get; set; }

        public string Fraccion { get; set; }

        public string Descripcion { get; set; }

        public double Valor { get; set; }

        public double Peso { get; set; }

        public int bultos { get; set; }

        public string ClavePedimento { get; set; }

        public string Regimen { get; set; }

        public string Guias { get; set; }

        public string Prorrateo { get; set; }

        public string PreDODA { get; set; }
    }
}
