using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class CIntegracionDODAPITA
    {
        private string Rfc;
        private int DespachoAduanero;
        private string NumeroGafeteUnico;
        private int Aduana;
        private string Seccion;
        private string CAAT;
        private string IdTransporte;
        private string FastId;
        private string TipoOperacion;
        private List<CContenedores> LContenedores;
        private List<CPedimentoAmericano> LPedimentoAmericano;
        private List<CPedimento> LPedimento;
        private CSellado Sellado;
        private string Patente;
        private List<DodaRemesa> ListaIdReferenciaField;
        private string Placas;
        private string NoAduana;
        private string CadenaOrigial;
        private CatalogodeSellosDigitales Sellos;
        public string _Rfc
        {
            get
            {
                return Rfc;
            }
            set
            {
                Rfc = value;
            }
        }
        public int _DespachoAduaneroPITA
        {
            get
            {
                return DespachoAduanero;
            }
            set
            {
                DespachoAduanero = value;
            }
        }
        public string _NumeroGafeteUnicoPITA
        {
            get
            {
                return NumeroGafeteUnico;
            }
            set
            {
                NumeroGafeteUnico = value;
            }
        }
        public int _Aduana
        {
            get
            {
                return Aduana;
            }
            set
            {
                Aduana = value;
            }
        }
        public string _Seccion
        {
            get
            {
                return Seccion;
            }
            set
            {
                Seccion = value;
            }
        }
        public string _CAAT
        {
            get
            {
                return CAAT;
            }
            set
            {
                CAAT = value;
            }
        }
        public string _IdTranporte
        {
            get
            {
                return IdTransporte;
            }
            set
            {
                IdTransporte = value;
            }
        }
        public string _FastaId
        {
            get
            {
                return FastId;
            }
            set
            {
                FastId = value;
            }
        }
        public string _TipoOperacion
        {
            get
            {
                return TipoOperacion;
            }
            set
            {
                TipoOperacion = value;
            }
        }
        public List<CContenedores> _LContenedores
        {
            get
            {
                return LContenedores;
            }
            set
            {
                LContenedores = value;
            }
        }
        public List<CPedimentoAmericano> _LPedimentoAmericano
        {
            get
            {
                return LPedimentoAmericano;
            }
            set
            {
                LPedimentoAmericano = value;
            }
        }
        public List<CPedimento> _LPedimento
        {
            get
            {
                return LPedimento;
            }
            set
            {
                LPedimento = value;
            }
        }
        public CSellado _Sellado
        {
            get
            {
                return Sellado;
            }
            set
            {
                Sellado = value;
            }
        }
        public string _Patente
        {
            get
            {
                return Patente;
            }
            set
            {
                Patente = value;
            }
        }
        public List<DodaRemesa> _ListaIdReferencia
        {
            get
            {
                return ListaIdReferenciaField;
            }
            set
            {
                ListaIdReferenciaField = value;
            }
        }
        public string _Placas
        {
            get
            {
                return Placas;
            }
            set
            {
                Placas = value;
            }
        }
        public string _NoAduana
        {
            get
            {
                return NoAduana;
            }
            set
            {
                NoAduana = value;
            }
        }
        public string _CadenaOriginal
        {
            get
            {
                return CadenaOrigial;
            }
            set
            {
                CadenaOrigial = value;
            }
        }

        public CatalogodeSellosDigitales _Sellos
        {
            get
            {
                return Sellos;
            }
            set
            {
                Sellos = value;
            }
        }
        public List<string> CFDIsCP { get; set; }
    }
}
