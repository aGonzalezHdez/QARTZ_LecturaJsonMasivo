using Microsoft.VisualBasic;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCasa
{
    public class TotalDeFacturaParaCOVE
    {
        private double TotalValorFacturaValue;
        public double TotalValorFactura
        {
            get
            {
                return TotalValorFacturaValue;
            }
            set
            {
                TotalValorFacturaValue = value;
            }
        }

        private double TotalValorPartidasFacturaValue;
        public double TotalValorPartidasFactura
        {
            get
            {
                return TotalValorPartidasFacturaValue;
            }
            set
            {
                TotalValorPartidasFacturaValue = value;
            }
        }

        private double DiferenciaFacturaValue;
        public string DiferenciaFactura
        {
            get
            {
                return DiferenciaFacturaValue.ToString();
            }
            set
            {
                DiferenciaFacturaValue = Convert.ToDouble(value);
            }
        }

        private double TotalPartidasFacturaValue;
        public string TotalPartidasFactura
        {
            get
            {
                return TotalPartidasFacturaValue.ToString();
            }
            set
            {
                TotalPartidasFacturaValue = Convert.ToDouble(value);
            }
        }

        private double TotalPesoValue;
        public string TotalPeso
        {
            get
            {
                return TotalPesoValue.ToString();
            }
            set
            {
                TotalPesoValue = Convert.ToDouble(value);
            }
        }
    }
}
