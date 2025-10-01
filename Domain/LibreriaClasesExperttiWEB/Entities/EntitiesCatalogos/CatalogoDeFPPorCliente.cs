namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{

    public class CatalogoDeFPPorCliente
    {
        private int IDFPField;
        public int IDFP
        {
            get
            {
                return IDFPField;
            }
            set
            {
                IDFPField = value;
            }
        }


        private string RFCField;
        public string RFC
        {
            get
            {
                return RFCField;
            }
            set
            {
                RFCField = value;
            }
        }


        private string ClaveDePedimentoField;
        public string ClaveDePedimento
        {
            get
            {
                return ClaveDePedimentoField;
            }
            set
            {
                ClaveDePedimentoField = value;
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


        private bool AplicaDTAField;
        public bool AplicaDTA
        {
            get
            {
                return AplicaDTAField;
            }
            set
            {
                AplicaDTAField = value;
            }
        }


        private int FPDTAField;
        public int FPDTA
        {
            get
            {
                return FPDTAField;
            }
            set
            {
                FPDTAField = value;
            }
        }


        private bool AplicaIVAField;
        public bool AplicaIVA
        {
            get
            {
                return AplicaIVAField;
            }
            set
            {
                AplicaIVAField = value;
            }
        }


        private int FPIVAField;
        public int FPIVA
        {
            get
            {
                return FPIVAField;
            }
            set
            {
                FPIVAField = value;
            }
        }


        private bool AplicaADVField;
        public bool AplicaADV
        {
            get
            {
                return AplicaADVField;
            }
            set
            {
                AplicaADVField = value;
            }
        }


        private int FPADVField;
        public int FPADV
        {
            get
            {
                return FPADVField;
            }
            set
            {
                FPADVField = value;
            }
        }

    }
}
