namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonWec
{
    public class RevalidacionWECRespuesta
    {
        private string pDomRESTOkField;
        public string pDomRESTOk
        {
            get
            {
                return pDomRESTOkField;
            }
            set
            {
                pDomRESTOkField = value;
            }
        }

        private string pDomRESTMsgField;
        public string pDomRESTMsg
        {
            get
            {
                return pDomRESTMsgField;
            }
            set
            {
                pDomRESTMsgField = value;
            }
        }

        private string PatenteField;
        public string Patente
        {
            get
            {
                return PatenteField;
            }
            set
            {
                PatenteField = value;
            }
        }

        private string pPDFBase64Field;
        public string pPDFBase64
        {
            get
            {
                return pPDFBase64Field;
            }
            set
            {
                pPDFBase64Field = value;
            }
        }
    }
}
