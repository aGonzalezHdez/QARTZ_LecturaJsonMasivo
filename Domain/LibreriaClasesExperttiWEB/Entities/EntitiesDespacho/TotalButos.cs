using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class TotalButos
    {
        public int TotaldeBultos { get; set; }
        public int EnUnidad { get; set; }
        public int Faltan { get; set; }
    }
    public class BultosPedimento
    {
        public string Pedimento { get; set; }
        public int BultosenPedimento { get; set; }
        public int EnUnidad { get; set; }
        public int Faltan { get; set; }
    }
}
