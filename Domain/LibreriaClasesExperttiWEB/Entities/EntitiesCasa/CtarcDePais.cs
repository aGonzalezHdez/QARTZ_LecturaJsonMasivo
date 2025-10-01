namespace LibreriaClasesAPIExpertti.Entities.EntitiesCasa
{
    public class CtarcDePais
    {
        private string CVE_PAIField;
        public string CVE_PAI
        {
            get
            {
                return CVE_PAIField;
            }
            set
            {
                CVE_PAIField = value;
            }
        }


        private string MON_PAIField;
        public string MON_PAI
        {
            get
            {
                return MON_PAIField;
            }
            set
            {
                MON_PAIField = value;
            }
        }


        private DateTime FEC_DOFField;
        public DateTime FEC_DOF
        {
            get
            {
                return FEC_DOFField;
            }
            set
            {
                FEC_DOFField = value;
            }
        }


        private double EQU_DLLSField;
        public double EQU_DLLS
        {
            get
            {
                return EQU_DLLSField;
            }
            set
            {
                EQU_DLLSField = value;
            }
        }


        private DateTime VIG_HASTField;
        public DateTime VIG_HAST
        {
            get
            {
                return VIG_HASTField;
            }
            set
            {
                VIG_HASTField = value;
            }
        }
    }
}
