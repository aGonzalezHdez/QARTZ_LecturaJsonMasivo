namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class ent_DODA
    {
        private int IdDODA;
        private string N_Ticket;
        private string N_Integracion;
        private string Patente;
        private DateTime FechaEmision;
        private int N_Pedimentos;
        private int NAduana;
        private string NoAduana;
        private string Seccion;
        private string CadenaOriginal;
        private string NSerieCertificado;
        private string SelloDigital;
        private string NSerieCertificadoSAT;
        private string SelloDigitalSAT;
        private string CadenaOriginalSAT;
        private string UsuarioCiec;
        private string Link;
        private string RutaArchivo;
        private ent_ListaDODA ListaDoda;
        private byte[] Img;
        private string NumeroGafeteUnico;
        private int DespachoAduanero;
        private CIntegracionDODAPITA ent_Lista_DODANew;


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

        public string _N_Integracion
        {
            get
            {
                return N_Integracion;
            }
            set
            {
                N_Integracion = value;
            }
        }

        public int _IdDODA
        {
            get
            {
                return IdDODA;
            }
            set
            {
                IdDODA = value;
            }
        }

        public string _N_Ticket
        {
            get
            {
                return N_Ticket;
            }
            set
            {
                N_Ticket = value;
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

        public DateTime _FechaEmision
        {
            get
            {
                return FechaEmision;
            }
            set
            {
                FechaEmision = value;
            }
        }

        public int _N_Pedimentos
        {
            get
            {
                return N_Pedimentos;
            }
            set
            {
                N_Pedimentos = value;
            }
        }

        public int _NAduana
        {
            get
            {
                return NAduana;
            }
            set
            {
                NAduana = value;
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
                return CadenaOriginal;
            }
            set
            {
                CadenaOriginal = value;
            }
        }

        public string _NSerieCertificado
        {
            get
            {
                return NSerieCertificado;
            }
            set
            {
                NSerieCertificado = value;
            }
        }

        public string _SelloDigital
        {
            get
            {
                return SelloDigital;
            }
            set
            {
                SelloDigital = value;
            }
        }

        public string _NSerieCertificadoSAT
        {
            get
            {
                return NSerieCertificadoSAT;
            }
            set
            {
                NSerieCertificadoSAT = value;
            }
        }

        public string _SelloDigitalSAT
        {
            get
            {
                return SelloDigitalSAT;
            }
            set
            {
                SelloDigitalSAT = value;
            }
        }

        public string _CadenaOriginalSAT
        {
            get
            {
                return CadenaOriginalSAT;
            }
            set
            {
                CadenaOriginalSAT = value;
            }
        }

        public string _Link
        {
            get
            {
                return Link;
            }
            set
            {
                Link = value;
            }
        }

        public string _UsuarioCiec
        {
            get
            {
                return UsuarioCiec;
            }
            set
            {
                UsuarioCiec = value;
            }
        }

        public string _RutaArchivo
        {
            get
            {
                return RutaArchivo;
            }
            set
            {
                RutaArchivo = value;
            }
        }

        public ent_ListaDODA _ent_Lista_DODA
        {
            get
            {
                return ListaDoda;
            }
            set
            {
                ListaDoda = value;
            }
        }

        public byte[] _Img
        {
            get
            {
                return Img;
            }
            set
            {
                Img = value;
            }
        }

        public string _NumeroGafeteUnico
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

        public int _DespachoAduanero
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

        private bool DetenerField;
        public bool Detener
        {
            get
            {
                return DetenerField;
            }
            set
            {
                DetenerField = value;
            }
        }

        private string PlacasField;
        public string Placas
        {
            get
            {
                return PlacasField;
            }
            set
            {
                PlacasField = value;
            }
        }

        private string CAATField;
        public string CAAT
        {
            get
            {
                return CAATField;
            }
            set
            {
                CAATField = value;
            }
        }

        private int IdPredodaField;
        public int IdPredoda
        {
            get
            {
                return IdPredodaField;
            }
            set
            {
                IdPredodaField = value;
            }
        }

        private string ArchivoField;
        public string Archivo
        {
            get
            {
                return ArchivoField;
            }
            set
            {
                ArchivoField = value;
            }
        }


        public CIntegracionDODAPITA _ent_Lista_DODANew
        {
            get
            {
                return ent_Lista_DODANew;
            }
            set
            {
                ent_Lista_DODANew = value;
            }
        }

        private string NumerodeTagField;
        public string NumerodeTag
        {
            get
            {
                return NumerodeTagField;
            }
            set
            {
                NumerodeTagField = value;
            }
        }


        private int tipo_documento_idField;
        public int tipo_documento_id
        {
            get
            {
                return tipo_documento_idField;
            }
            set
            {
                tipo_documento_idField = value;
            }
        }


        private string Fast_IdField;
        public string Fast_Id
        {
            get
            {
                return Fast_IdField;
            }
            set
            {
                Fast_IdField = value;
            }
        }


        private string datos_adicionalesField;
        public string datos_adicionales
        {
            get
            {
                return datos_adicionalesField;
            }
            set
            {
                datos_adicionalesField = value;
            }
        }


        private int ModalidadCruceField;
        public int ModalidadCruce
        {
            get
            {
                return ModalidadCruceField;
            }
            set
            {
                ModalidadCruceField = value;
            }
        }


        private string CandadosField;
        public string Candados
        {
            get
            {
                return CandadosField;
            }
            set
            {
                CandadosField = value;
            }
        }


        private string AVCField;
        public string AVC
        {
            get
            {
                return AVCField;
            }
            set
            {
                AVCField = value;
            }
        }


        private DateTime FechaVigenciaField;
        public DateTime FechaVigencia
        {
            get
            {
                return FechaVigenciaField;
            }
            set
            {
                FechaVigenciaField = value;
            }
        }


        private string ValidacionAgenciaField;
        public string ValidacionAgencia
        {
            get
            {
                return ValidacionAgenciaField;
            }
            set
            {
                ValidacionAgenciaField = value;
            }
        }


        private int OperacionField;
        public int Operacion
        {
            get
            {
                return OperacionField;
            }
            set
            {
                OperacionField = value;
            }
        }
    }
}
