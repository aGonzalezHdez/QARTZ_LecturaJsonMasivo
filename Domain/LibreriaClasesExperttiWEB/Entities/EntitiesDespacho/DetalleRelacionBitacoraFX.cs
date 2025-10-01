using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class DetalleRelacionBitacoraFX
    {
        public string Tipo { get; set; }
        public string Cliente { get; set; }
        public string Guia { get; set; } 
        public string Referencia { get; set; }
        public double Valor { get; set; }
        public double Peso { get; set; }
        public int Bultos { get; set; }
        public string Pedimento { get; set; }
    }
}
