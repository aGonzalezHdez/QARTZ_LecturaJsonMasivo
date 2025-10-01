namespace LibreriaClasesAPIExpertti.Entities.EntitiesXMLGrupoEI
{
    public class XMLEi
    {
        private string GuiaHouseField;
        public string GuiaHouse
        {
            get
            {
                return GuiaHouseField;
            }
            set
            {
                GuiaHouseField = value;
            }
        }

        private XMLEiRiel RielField;
        public XMLEiRiel Riel
        {
            get
            {
                return RielField;
            }
            set
            {
                RielField = value;
            }
        }

        private string IATAOrigenField;
        public string IATAOrigen
        {
            get
            {
                return IATAOrigenField;
            }
            set
            {
                IATAOrigenField = value;
            }
        }

        private string IATADestinoField;
        public string IATADestino
        {
            get
            {
                return IATADestinoField;
            }
            set
            {
                IATADestinoField = value;
            }
        }

        private int BultosField;
        public int Bultos
        {
            get
            {
                return BultosField;
            }
            set
            {
                BultosField = value;
            }
        }

        private double ValorDolaresField;
        public double ValorDolares
        {
            get
            {
                return ValorDolaresField;
            }
            set
            {
                ValorDolaresField = value;
            }
        }

        private XMLEiCliente ClienteField;
        public XMLEiCliente Cliente
        {
            get
            {
                return ClienteField;
            }
            set
            {
                ClienteField = value;
            }
        }
    }
}
