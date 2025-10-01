using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public partial class ComponentesGenerales
    {

        private Referencias ReferenciaField;
        public Referencias Referencia
        {
            get
            {
                return ReferenciaField;
            }
            set
            {
                ReferenciaField = value;
            }
        }

        private SaaioPedime PedimeField;
        public SaaioPedime Pedime
        {
            get
            {
                return PedimeField;
            }
            set
            {
                PedimeField = value;
            }
        }

        private int TipodeFiguraField;
        public int TipodeFigura
        {
            get
            {
                return TipodeFiguraField;
            }
            set
            {
                TipodeFiguraField = value;
            }
        }

        private CtracClient ClienteCasaField;
        public CtracClient ClienteCasa
        {
            get
            {
                return ClienteCasaField;
            }
            set
            {
                ClienteCasaField = value;
            }
        }

        private Clientes ClienteExperttiField;
        public Clientes ClienteExpertti
        {
            get
            {
                return ClienteExperttiField;
            }
            set
            {
                ClienteExperttiField = value;
            }
        }


        private string tipoOperacionField;
        public string tipoOperacion
        {
            get
            {
                return tipoOperacionField;
            }
            set
            {
                tipoOperacionField = value;
            }
        }


        private string[] RfcConsultaField;
        public string[] RfcConsulta
        {
            get
            {
                return RfcConsultaField;
            }
            set
            {
                RfcConsultaField = value;
            }
        }

        public CatalogodeSellosDigitales Sello { get; set; }

    }

}