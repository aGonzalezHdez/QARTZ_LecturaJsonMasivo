using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class ArchivoErr
    {
        private string _situacion;
        public string Situacion
        {
            get { return _situacion; }
            set { _situacion = value; }
        }

        private string _pedimento;
        public string Pedimento
        {
            get { return _pedimento; }
            set { _pedimento = value; }
        }

        private string _firmaElectronica;
        public string FirmaElectronica
        {
            get { return _firmaElectronica; }
            set { _firmaElectronica = value; }
        }

        private string _impeError;
        public string ImpeError
        {
            get { return _impeError; }
            set { _impeError = value; }
        }

        private string _solucionExpertti;
        public string SolucionExpertti
        {
            get { return _solucionExpertti; }
            set { _solucionExpertti = value; }
        }

        private string _claveimpeError;
        public string ClaveimpeError
        {
            get { return _claveimpeError; }
            set { _claveimpeError = value; }
        }

        private string _lineaDeCaptura;
        public string LineaDeCaptura
        {
            get { return _lineaDeCaptura; }
            set { _lineaDeCaptura = value; }
        }

        private string _impuestos;
        public string Impuestos
        {
            get { return _impuestos; }
            set { _impuestos = value; }
        }
    }

}
