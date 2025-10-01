using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJCJFSalidas
{
    public class SolicitaSalida
    {
        public string pSalpedimento { get; set; }

        public string pSalpedimentocve { get; set; }

        public string pSaloperacion { get; set; }

        public string pSalidaspatente { get; set; }

        public string pSalMaster { get; set; }

        public string pSalHouse { get; set; }

        public double pSalPeso { get; set; }

        public int pSalbultos { get; set; }

        public double pTransValorPedimento { get; set; }

        public DateTime pSalpfecha { get; set; }

        public string TipodePedimento { get; set; }

        public string RFC { get; set; }

        public DateTime pSalpfechaModulacion { get; set; }

        public string pSalRemitente { get; set; }

        public string pSalRemitenteDir { get; set; }

        public string pSalConsignatario { get; set; }

        public string pSalConsignatarioDir { get; set; }

        public string pSalDestino { get; set; }

        public string pSalDescripcion { get; set; }


    }
}
