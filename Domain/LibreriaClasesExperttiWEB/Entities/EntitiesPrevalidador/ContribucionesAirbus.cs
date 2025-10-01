using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class ContribucionesAirbus
    {
        private string _clave;
        public string Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }

        private string _claveTipoTasa;
        public string ClaveTipoTasa
        {
            get { return _claveTipoTasa; }
            set { _claveTipoTasa = value; }
        }

        private double _tasaAplicable;
        public double TasaAplicable
        {
            get { return _tasaAplicable; }
            set { _tasaAplicable = value; }
        }

        private string _nombreContribucion;
        public string NombreContribucion
        {
            get { return _nombreContribucion; }
            set { _nombreContribucion = value; }
        }

        private double _importe;
        public double Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        private string _formaDePago;
        public string FormaDePago
        {
            get { return _formaDePago; }
            set { _formaDePago = value; }
        }
    }

}
