namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class ent_ListaDODA
    {
        private string Rfc;
        private string FastId;
        private string TipoOperacion;
        private string Aduana;
        private string Seccion;
        private string Placas;
        private string CAAT;
        private string CadenaConexion;
        private string Patente;
        private string NumeroGafeteUnico;
        private int DespachoAduanero;

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

        public string _Aduana
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

        public string _CadenaConexion
        {
            get
            {
                return CadenaConexion;
            }
            set
            {
                CadenaConexion = value;

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

        private List<DodaRemesa> ListaIdReferenciaField;

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
    }
}
